using System;
using System.Windows.Input;

namespace Calculator
{
	/// <summary>
	/// デリゲートを受け取るICommandの実装
	/// </summary>
	class DelegateCommand : ICommand
	{
		private readonly Action execute;
		private readonly Action<object> argExecute;
		private readonly Func<bool> canExecute;
		private readonly Func<object, bool> canArgExecute;

		/// <summary>
		/// コマンドのExecuteメソッドで実行する処理とCanExecuteメソッドで実行する処理を指定して
		/// DelegateCommandのインスタンスを作成します。
		/// </summary>
		/// <param name="p_execute">Executeメソッドで実行する処理</param>
		/// <param name="p_canExecute">CanExecuteメソッドで実行する処理</param>
		public DelegateCommand(Action p_execute, Func<bool> p_canExecute)
		{
			this.execute = p_execute ?? throw new ArgumentNullException("execute");
			this.canExecute = p_canExecute ?? throw new ArgumentNullException("canExecute");
		}

		public DelegateCommand(Action p_execute) : this(p_execute, () => true) { }

		public DelegateCommand(Action<object> argExecute, Func<bool> canExecute)
		{
			this.argExecute = argExecute ?? throw new ArgumentNullException("execute");
			this.canExecute = canExecute ?? throw new ArgumentNullException("canExecute");
		}

		/// <summary>
		/// コマンドのExecuteメソッドで実行する処理とCanExecuteメソッドで実行する処理を指定して
		/// DelegateCommandのインスタンスを作成します。
		/// </summary>
		/// <param name="p_argExecute">Executeメソッドで実行する処理(引数有り)</param>
		/// <param name="p_canArgExecute">CanExecuteメソッドで実行する処理(引数有り)</param>
		public DelegateCommand(Action<object> p_argExecute, Func<object, bool> p_canArgExecute)
		{
			this.argExecute = p_argExecute ?? throw new ArgumentNullException("execute");
			this.canArgExecute = p_canArgExecute ?? throw new ArgumentNullException("canExecute");
		}

		/// <summary>
		/// ICommand.CanExecuteの明示的な実装。CanExecuteメソッドに処理を委譲する。
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		bool ICommand.CanExecute(object parameter)
		{
			if (this.canExecute != null) return this.canExecute();
			if (this.canArgExecute != null) return this.canArgExecute(parameter);

			throw new ArgumentNullException("canExecute");
		}

		/// <summary>
		/// CanExecuteの結果に変更があったことを通知するイベント。
		/// WPFのCommandManagerのRequerySuggestedイベントをラップする形で実装しています。
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		/// <summary>
		/// CanExecuteChangedを強制的に発生させる
		/// </summary>
		public static void RaiseCanExecuteChanged()
		{
			CommandManager.InvalidateRequerySuggested();
		}

		/// <summary>
		/// ICommand.Executeの明示的な実装。Executeメソッドに処理を委譲する。
		/// </summary>
		/// <param name="parameter"></param>
		void ICommand.Execute(object parameter)
		{
			this.execute?.Invoke();
			this.argExecute?.Invoke(parameter);
		}
	}
}

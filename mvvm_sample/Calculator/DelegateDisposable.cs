using System;

namespace Calculator
{
	/// <summary>
	/// 解放処理の汎用クラス
	/// </summary>
	sealed class DelegateDisposable : IDisposable
	{
		/// <summary>
		/// 解放処理
		/// </summary>
		private readonly Action disposeAction;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="p_disposeAction">解放処理</param>
		public DelegateDisposable(Action p_disposeAction)
		{
			this.disposeAction = p_disposeAction;
		}

		/// <summary>
		/// 解放
		/// </summary>
		public void Dispose()
		{
			this.disposeAction?.Invoke();
		}
	}
}

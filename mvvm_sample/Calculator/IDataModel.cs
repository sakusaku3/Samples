using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Calculator
{
	/// <summary>
	/// DataModel用のインターフェース
	/// PropertyChangedと入力検証の機能を提供する
	/// </summary>
	interface IDataModel : INotifyPropertyChanged
	{
		/// <summary>
		/// PropertyChangedの再定義
		/// (INotifyPropertyChangedのPropertyChangedを隠蔽する)
		/// </summary>
		new event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// PropertyChangedEventHandler の取得
		/// </summary>
		PropertyChangedEventHandler PropertyChangedHandler { get; }
	}

	/// <summary>
	/// IDataModelインターフェイスについてPropertyChagedの拡張メソッド提供
	/// </summary>
	static class IExtendedNotifyPropertyChangedFunctions
	{
		/// <summary>
		/// プロパティの変更の通知
		/// </summary>
		/// <param name="p_self">対象</param>
		/// <param name="targets">プロパティ変更通知対象の名称群</param>
		public static void OnPropertyChanged(
			this IDataModel p_self,
			[CallerMemberName] string p_propertyName = null)
		{
			if (p_self == null) return;
			if (p_self.PropertyChangedHandler == null) return;
			p_self.PropertyChangedHandler(p_self, new PropertyChangedEventArgs(p_propertyName));
		}

		/// <summary>
		/// プロパティの変更と、イベントの発生を担うメソッド
		/// </summary>
		/// <typeparam name="T">イベントを発生させるViewModel</typeparam>
		/// <param name="p_self">ViewMOdelのインスタンス</param>
		/// <param name="p_storage">変更前の値</param>
		/// <param name="p_value">変更後の値</param>
		/// <param name="p_propertyName">プロパティの名前</param>
		/// <returns></returns>
		public static bool SetProperty<TProperty>(
			this IDataModel p_self,
			ref TProperty p_storage,
			TProperty p_value,
			[CallerMemberName] string p_propertyName = null)
		{
			// プロパティの変更前と後が同じであればイベントは発生させない
			if (object.Equals(p_storage, p_value)) return false;
			p_storage = p_value;
			OnPropertyChanged(p_self, p_propertyName);
			return true;
		}
	}

	/// <summary>
	/// INotifyPropertyChangedインターフェイスについて拡張メソッドを提供
	/// </summary>
	static class INotifyPropertyChangedFunction
	{
		/// <summary>
		/// senderオブジェクトへ観察対象のプロパティを指定してlistenerオブジェクトのメソッドを登録する
		/// </summary>
		/// <param name="p_propertyName">観察対象のプロパティ</param>
		/// <param name="p_handler">通知時の実行メソッド</param>
		static public IDisposable AddPropertyChanged<TObject>(
			this TObject p_self,
			string p_propertyName,
			Action<TObject> p_handler)
			where TObject : INotifyPropertyChanged
		{
			// 追加/解除対象のイベントハンドラーを用意しておく
			// このオブジェクトを追加/解除時で、同じものを利用しないと
			// senderオブジェクトから解除できずにイベントハンドラーが残ってしまう
			PropertyChangedEventHandler l_handler =
				(sender, e) =>
				{ if (e.PropertyName == p_propertyName) { p_handler(p_self); } };

			// イベントハンドラーをsenderへ追加する
			p_self.PropertyChanged += l_handler;

			// listenerが破棄される際に、PropertyChangedのチェーンを確実に解除するために
			// イベントハンドラーをsenderから解除するメソッドを隠蔽化した破棄対象オブジェクトを生成して返す
			// Disposeはlistenerに任せる
			return new DelegateDisposable(() => p_self.PropertyChanged -= l_handler);
		}

		/// <summary>
		/// senderオブジェクトへ観察対象のプロパティを指定してlistenerオブジェクトのメソッドを登録する
		/// </summary>
		/// <param name="p_propertyName">観察対象のプロパティ</param>
		/// <param name="p_handler">通知時の実行メソッド</param>
		public static IDisposable AddPropertyChanged<TObject>(
			this TObject p_self,
			string p_propertyName,
			Action p_handler)
			where TObject : INotifyPropertyChanged
		{
			// 追加/解除対象のイベントハンドラーを用意しておく
			// このオブジェクトを追加/解除時で、同じものを利用しないと
			// senderオブジェクトから解除できずにイベントハンドラーが残ってしまう
			PropertyChangedEventHandler l_handler =
				(sender, e) =>
				{ if (e.PropertyName == p_propertyName) { p_handler(); } };

			// イベントハンドラーをsenderへ追加する
			p_self.PropertyChanged += l_handler;

			// listenerが破棄される際に、PropertyChangedのチェーンを確実に解除するために
			// イベントハンドラーをsenderから解除するメソッドを隠蔽化した破棄対象オブジェクトを生成して返す
			// Disposeはlistenerに任せる
			return new DelegateDisposable(() => p_self.PropertyChanged -= l_handler);
		}
	}

	static class INotifyCollectionChangeFunction
	{
		/// <summary>
		/// senderオブジェクトへ観察対象のプロパティを指定してlistenerオブジェクトのメソッドを登録する
		/// </summary>
		/// <param name="p_propertyName">観察対象のプロパティ</param>
		/// <param name="p_handler">通知時の実行メソッド</param>
		static public IDisposable AddCollectionChanged<TObject>(
			this TObject p_self,
			Action p_handler)
			where TObject : INotifyCollectionChanged
		{
			// 追加/解除対象のイベントハンドラーを用意しておく
			// このオブジェクトを追加/解除時で、同じものを利用しないと
			// senderオブジェクトから解除できずにイベントハンドラーが残ってしまう
			NotifyCollectionChangedEventHandler l_handler =
				(sender, e) => p_handler.Invoke();

			// イベントハンドラーをsenderへ追加する
			p_self.CollectionChanged += l_handler;

			// listenerが破棄される際に、PropertyChangedのチェーンを確実に解除するために
			// イベントハンドラーをsenderから解除するメソッドを隠蔽化した破棄対象オブジェクトを生成して返す
			// Disposeはlistenerに任せる
			return new DelegateDisposable(() => p_self.CollectionChanged -= l_handler);
		}
	}
}

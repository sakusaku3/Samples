using System;
using System.ComponentModel;

namespace Calculator
{
    /// <summary>
    /// INotifyPropertyChangedインターフェイスについて拡張メソッドを提供
    /// </summary>
    static class INotifyPropertyChangedExtensions
	{
		/// <summary>
		/// senderオブジェクトへ観察対象のプロパティを指定してlistenerオブジェクトのメソッドを登録する
		/// </summary>
		/// <param name="p_propertyName">観察対象のプロパティ</param>
		/// <param name="p_handler">通知時の実行メソッド</param>
		public static IDisposable AddPropertyChanged<TObject>(
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

}

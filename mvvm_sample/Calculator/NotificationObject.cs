using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Calculator
{
    class NotificationObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// プロパティの変更の通知
		/// </summary>
		public void OnPropertyChanged(
			[CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(
				this, new PropertyChangedEventArgs(propertyName));
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
		public bool SetProperty<TProperty>(
			ref TProperty p_storage,
			TProperty p_value,
			[CallerMemberName] string p_propertyName = null)
		{
			// プロパティの変更前と後が同じであればイベントは発生させない
			if (object.Equals(p_storage, p_value)) return false;
			p_storage = p_value;
			this.OnPropertyChanged(p_propertyName);
			return true;
		}
    }
}

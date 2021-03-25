namespace Calculator.Models
{
    /// <summary>
    /// 項クラス
    /// </summary>
    class Term : NotificationObject
    {
        /// <summary>
        /// 値
        /// </summary>
        public int Value
        {
            get { return this._value; }
            set { this.SetProperty(ref this._value, value); }
        }
        private int _value;
    }
}

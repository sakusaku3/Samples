namespace Calculator.Models
{
    class Calculators : NotificationObject
    {
        /// <summary>
        /// X
        /// </summary>
        public int X 
        {
            get { return this._x; }
            set { this.SetProperty(ref this._x, value); }
        }
        private int _x;

        /// <summary>
        /// Y
        /// </summary>
        public int Y 
        {
            get { return this._y; }
            set { this.SetProperty(ref this._y, value); }
        }
        private int _y;

        /// <summary>
        /// Result
        /// </summary>
        public int Result
        {
            get { return this._result; }
            set { this.SetProperty(ref this._result, value); }
        }
        private int _result;

        public void AddExecute()
        {
            this.Result = this.X + this.Y;
        }

        public bool CanAddExecute()
        {
            return true;
        }
    }
}

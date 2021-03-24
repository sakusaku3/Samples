using System.Windows;

namespace Calculator.ViewModels
{
    public class MainViewModel : NotificationObject
    {
        /// <summary>
        /// X
        /// </summary>
        public int X 
        {
            get { return this.model.X; }
            set { this.model.X = value; }
        }

        /// <summary>
        /// Y
        /// </summary>
        public int Y 
        {
            get { return this.model.Y; }
            set { this.model.Y = value; }
        }

        /// <summary>
        /// Result 
        /// </summary>
        public int Result 
        {
            get { return this.model.Result; }
            set { this.model.Result = value; }
        }

        /// <summary>
        /// 足す
        /// </summary>
        public DelegateCommand AddCommand
        {
            get
            {
                return this._addCommand ??= new DelegateCommand(
                      this.model.AddExecute,
                      this.model.CanAddExecute);
            }
        }
        private DelegateCommand _addCommand;

        public DelegateCommand HelloCommand
        {
            get
            {
                return this._helloCommand ??= new DelegateCommand(
                      () => this.HelloExecute(),
                      () => true);
            }
        }
        private DelegateCommand _helloCommand;

        public Messenger Messenger { get; } = new Messenger();

        public void HelloExecute()
        {
            var msg = new DialogBoxMessage(this);
            msg.Message = "さん、こんにちは。";
            msg.Button = MessageBoxButton.YesNo;
            this.Messenger.Send(this, msg);
            if (msg.Result == MessageBoxResult.Yes)
            {
                this.model.X = 2;
            }
        }

        private readonly Models.Calculators model;

        public MainViewModel() 
        {
            this.model = new Models.Calculators();

            this.model.AddPropertyChanged(
                nameof(this.model.X), () => this.OnPropertyChanged(nameof(this.X)));

            this.model.AddPropertyChanged(
                nameof(this.model.Y), () => this.OnPropertyChanged(nameof(this.Y)));

            this.model.AddPropertyChanged(
                nameof(this.model.Result), () => this.OnPropertyChanged(nameof(this.Result)));
        }
    }
}

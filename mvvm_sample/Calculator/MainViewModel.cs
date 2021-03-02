using System.ComponentModel;

namespace Calculator
{
    public class MainViewModel : IDataModel
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

        private readonly Calculators model;

        public MainViewModel() 
        {
            this.model = new Calculators();

            this.model.AddPropertyChanged(
                nameof(this.model.X), () => this.OnPropertyChanged(nameof(this.X)));

            this.model.AddPropertyChanged(
                nameof(this.model.Y), () => this.OnPropertyChanged(nameof(this.Y)));

            this.model.AddPropertyChanged(
                nameof(this.model.Result), () => this.OnPropertyChanged(nameof(this.Result)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        PropertyChangedEventHandler IDataModel.PropertyChangedHandler
        {
            get { return this.PropertyChanged; }
        }
    }
}

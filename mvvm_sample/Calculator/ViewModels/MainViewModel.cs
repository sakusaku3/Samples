using System.Collections.ObjectModel;

namespace Calculator.ViewModels
{
    class MainViewModel : NotificationObject
    {
        #region Xaml公開プロパティ
        /// <summary>
        /// 項リスト
        /// </summary>
        public ObservableCollection<TermViewModel> Terms { get; } 
            = new ObservableCollection<TermViewModel>();

        /// <summary>
        /// 結果
        /// </summary>
        public int Result 
        {
            get { return this.model.Result; }
            set { this.model.Result = value; }
        }
        #endregion

        #region Xaml公開コマンド
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

        /// <summary>
        /// 項追加
        /// </summary>
        public DelegateCommand AddTermCommand
        {
            get
            {
                return this._addTermCommand ??= new DelegateCommand(
                    this.model.AddNewTerm);
            }
        }
        private DelegateCommand _addTermCommand;

        /// <summary>
        /// 項削除
        /// </summary>
        public DelegateCommand DeleteTermCommand
        {
            get
            {
                return this._deleteTermCommand ??= new DelegateCommand(
                    this.model.DeleteTerm,
                    this.model.CanDeleteTerm);
            }
        }
        private DelegateCommand _deleteTermCommand;
        #endregion

        #region フィールド
        /// <summary>
        /// 加算器モデル
        /// </summary>
        private readonly Models.Adder model;
        #endregion

        #region コンストラクタ
        public MainViewModel() 
        {
            this.model = new Models.Adder();

            this.model.AddPropertyChanged(
                nameof(this.model.Result), 
                () => this.OnPropertyChanged(nameof(this.Result)));

            this.Terms.SynchronizeWith(
                this.model.Terms,
                x => new TermViewModel(x));
        }
        #endregion
    }
}

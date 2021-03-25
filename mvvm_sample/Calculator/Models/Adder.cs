using System.Collections.ObjectModel;
using System.Linq;

namespace Calculator.Models
{
    /// <summary>
    /// 加算器クラス
    /// </summary>
    class Adder : NotificationObject
    {
        /// <summary>
        /// 項リスト
        /// </summary>
        public ObservableCollection<Term> Terms { get; } 
            = new ObservableCollection<Term>();

        /// <summary>
        /// 結果
        /// </summary>
        public int Result
        {
            get { return this._result; }
            set { this.SetProperty(ref this._result, value); }
        }
        private int _result;

        /// <summary>
        /// 項追加
        /// </summary>
        public void AddNewTerm()
        {
            this.Terms.Add(new Term());
        }

        /// <summary>
        /// 項削除
        /// </summary>
        public void DeleteTerm()
        {
            if (!this.Terms.Any()) return;
            this.Terms.Remove(this.Terms.Last());
        }

        /// <summary>
        /// 足し算実行可能
        /// </summary>
        /// <returns></returns>
        public bool CanDeleteTerm()
        {
            return this.Terms.Any();
        }

        /// <summary>
        /// 足し算実行
        /// </summary>
        public void AddExecute()
        {
            this.Result = this.Terms.Sum(x => x.Value);
        }

        /// <summary>
        /// 足し算実行可能
        /// </summary>
        /// <returns></returns>
        public bool CanAddExecute()
        {
            return true;
        }
    }
}

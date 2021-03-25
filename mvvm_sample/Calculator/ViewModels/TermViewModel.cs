using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.ViewModels
{
    /// <summary>
    /// 項のViewModel
    /// </summary>
    class TermViewModel : NotificationObject
    {
        /// <summary>
        /// 値
        /// </summary>
        public int Value
        {
            get { return this.model.Value; }
            set { this.model.Value = value; }
        }

        /// <summary>
        /// 項モデル
        /// </summary>
        private readonly Models.Term model;

        public TermViewModel(Models.Term p_model)
        {
            this.model = p_model;

            this.model.AddPropertyChanged(
                nameof(this.model.Value),
                () => this.OnPropertyChanged(nameof(this.Value)));
        }
    }
}

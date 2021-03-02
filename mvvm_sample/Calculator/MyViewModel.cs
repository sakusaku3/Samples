using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Calculator
{
    class MyViewModel : IDataModel
    {
        public string Name
        {
            get { return this._name; }
            set { this.SetProperty(ref this._name, value); }
        }
        private string _name;

        public bool IsOk => !string.IsNullOrWhiteSpace(this.Name);

        public ICommand HelloCommand
        {
            get
            {
                return this._helloCommand ??= new DelegateCommand(
                      () => this.HelloExecute(),
                      () => this.IsOk);
            }
        }
        private DelegateCommand _helloCommand;

        public void HelloExecute()
        {
            var msg = new DialogBoxMessage(this);
            msg.Message = Name + "さん、こんにちは。";
            msg.Button = MessageBoxButton.YesNo;
            // Messenger.Default.Send(this, msg);
            if (msg.Result == MessageBoxResult.Yes)
            {
                Name = "";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        PropertyChangedEventHandler IDataModel.PropertyChangedHandler
        {
            get { return this.PropertyChanged; }
        }
    }
}

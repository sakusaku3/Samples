using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Calculator
{
    class DialogBoxMessage : VmMessage
    {
        public string Message { get; set; }
        public MessageBoxButton Button { get; set; }
        public MessageBoxResult Result { get; set; }

        public DialogBoxMessage(object sender) : base(sender)
        {
        }
    }
}

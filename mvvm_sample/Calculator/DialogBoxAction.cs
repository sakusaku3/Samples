using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Calculator
{
    public class DialogBoxAction : IViewAction
    {
        private static void ShowMessage(DialogBoxMessage msg)
        {
            var result = MessageBox.Show(msg.Message, "確認", msg.Button);
            msg.Result = result;
        }

        public void Register(FrameworkElement recipient, Messenger messenger)
        {
            if (recipient is null)
            {
                throw new ArgumentNullException(nameof(recipient));
            }

            messenger.Register<DialogBoxMessage>(recipient, ShowMessage);
        }
    }
}

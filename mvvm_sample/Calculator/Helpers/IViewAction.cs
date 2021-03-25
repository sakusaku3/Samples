using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Calculator
{
    interface IViewAction
    {
        void Register(FrameworkElement recipient, Messenger messenger);
    }
}

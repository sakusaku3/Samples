using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Calculator
{
    public interface IViewAction
    {
        void Register(FrameworkElement recipient, Messenger messenger);
    }
}

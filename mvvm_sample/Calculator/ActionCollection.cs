using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace Calculator
{
    public class ActionCollection : Freezable
    {
        private Collection<IViewAction> _collection = new Collection<IViewAction>();

        public Collection<IViewAction> Collection
        {
            get { return _collection; }
        }

        public void RegisterAll(FrameworkElement Recipient)
        {
            Messenger mes = (SourceObject as Messenger);
            foreach (var action in this.Collection)
            {
                action.Register(Recipient, mes);
            }
        }

        public object SourceObject
        {
            get { return base.GetValue(SourceObjectProperty); }
            set { base.SetValue(SourceObjectProperty, value); }
        }

        public static readonly DependencyProperty SourceObjectProperty =
                DependencyProperty.Register("SourceObject", typeof(object), typeof(ActionCollection));

        protected override Freezable CreateInstanceCore()
        {
            return (Freezable)Activator.CreateInstance(base.GetType());
        }
    }
}

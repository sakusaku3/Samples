using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Calculator
{
    public class ActionCollection : Freezable
    {
        public Collection<IViewAction> Collection { get; } = new Collection<IViewAction>();

        public void RegisterAll(FrameworkElement recipient)
        {
            var mes = this.SourceObject as Messenger;
            foreach (var action in this.Collection)
            {
                action.Register(recipient, mes);
            }
        }

        public object SourceObject
        {
            get { return base.GetValue(SourceObjectProperty); }
            set { base.SetValue(SourceObjectProperty, value); }
        }

        public static readonly DependencyProperty SourceObjectProperty =
            DependencyProperty.Register(
                nameof(SourceObject),
                typeof(object), 
                typeof(ActionCollection));

        protected override Freezable CreateInstanceCore()
        {
            return (Freezable)Activator.CreateInstance(base.GetType());
        }
    }
}

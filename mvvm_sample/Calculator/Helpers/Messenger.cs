using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Calculator
{
    class Messenger
    {
        private readonly List<ActionInfo> list = new List<ActionInfo>();

        public void Register<TMessage>(
            FrameworkElement recipient, 
            Action<TMessage> action)
        {
            this.list.Add(new ActionInfo(
                typeof(TMessage),
                recipient.DataContext as INotifyPropertyChanged,
                action));
        }

        public void Send<TMessage>(
            INotifyPropertyChanged sender, 
            TMessage message)
        {
            var actions = this.list
                .Where(x => x.Sender == sender && x.Type == message.GetType())
                .Select(x => x.Action)
                .OfType<Action<TMessage>>();

            foreach (var action in actions)
                action.Invoke(message);
        }

        private class ActionInfo
        {
            public Type Type { get; }
            public INotifyPropertyChanged Sender { get; }
            public Delegate Action { get; }

            public ActionInfo(
                Type type,
                INotifyPropertyChanged sender,
                Delegate action)
            {
                this.Type = type;
                this.Sender = sender;
                this.Action = action;
            }
        }
    }
}

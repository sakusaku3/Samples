using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Calculator
{
    public class Messenger
    {
        private readonly List<ActionInfo> list = new List<ActionInfo>();

        public void Register<TMessage>(
            FrameworkElement recipient, 
            Action<TMessage> action)
        {
            this.list.Add(new ActionInfo
            {
                Type = typeof(TMessage),
                Sender = recipient.DataContext as INotifyPropertyChanged,
                Action = action,
            });
        }

        public void Send<TMessage>(
            INotifyPropertyChanged sender, 
            TMessage message)
        {
            var query = this.list
                .Where(x => x.Sender == sender && x.Type == message.GetType())
                .Select(x => x.Action as Action<TMessage>);

            foreach (var action in query)
            {
                action.Invoke(message);
            }
        }

        private class ActionInfo
        {
            public Type Type { get; set; }
            public INotifyPropertyChanged Sender { get; set; }
            public Delegate Action { get; set; }
        }
    }
}

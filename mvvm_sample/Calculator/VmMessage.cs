using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
    public class VmMessage
    {
        public object Sender { get; protected set; }

        public VmMessage(object sender)
        {
            this.Sender = sender;
        }
    }
}

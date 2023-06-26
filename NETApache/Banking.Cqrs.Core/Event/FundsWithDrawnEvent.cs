using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Cqrs.Core.Event
{
    public class FundsWithDrawnEvent : BaseEvent
    {
        public decimal Amount { get; set; }

        public FundsWithDrawnEvent(string id) : base(id)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rob.EventSourcing.Contracts.Messages;

namespace Rob.EventSourcing.Contracts.Bus
{
    public interface IHandle<TMessage> where TMessage : IMessage
    {
        void Handle(TMessage message);
    }
}

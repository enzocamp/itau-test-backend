using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Infrastructure.Messaging.Interfaces
{
    public interface IMessageProducer
    {
        Task SendAsync<T>(string topic, T message);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WandUser.Application.Producer
{
    public interface IKafkaProducer
    {
        Task SendMessageAsync(string topic, string message);
    }
}

using Akka.Actor;
using Akka.Cluster.Sharding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharding
{
    public class Message { }

    public class ShardingActor : ReceiveActor
    {
        public ShardingActor(string id)
        {
            Receive<Message>(_ =>
            {
                Console.WriteLine($"ShardingActor {id} received a message");
            });
            Receive<ReceiveTimeout>(_ => Context.Parent.Tell(new Passivate(PoisonPill.Instance)));

            SetReceiveTimeout(TimeSpan.FromSeconds(10));
        }
    }
}

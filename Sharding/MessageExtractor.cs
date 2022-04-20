using Akka.Cluster.Sharding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharding
{
    public sealed class ShardEnvelope
    {
        public string EntityId { get; }
        public object Payload { get; }

        public ShardEnvelope(string entityId, object payload)
        {
            EntityId = entityId;
            Payload = payload;
        }
    }

    public sealed class MessageExtractor : HashCodeMessageExtractor
    {
        public MessageExtractor(int maxNumberOfShards) : base(maxNumberOfShards)
        {
        }

        public override string? EntityId(object message)
        {
            switch (message)
            {
                case ShardRegion.StartEntity start:
                    return start.EntityId;
                case ShardEnvelope e:
                    return e.EntityId;
                default:
                    Console.WriteLine($"Should not be here {message.GetType()}");
                    return null;
            }
        }

        public override object EntityMessage(object message)
        {
            switch (message)
            {
                case ShardEnvelope e:
                    return e.Payload;
                default:
                    return message;
            }
        }
    }
}

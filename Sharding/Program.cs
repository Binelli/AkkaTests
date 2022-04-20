// See https://aka.ms/new-console-template for more information
using Akka.Actor;
using Akka.Cluster;
using Akka.Cluster.Sharding;
using Akka.Configuration;
using Sharding;

var config = ConfigurationFactory.ParseString(File.ReadAllText("akka.conf"));

var system = ActorSystem.Create("sharding", config);

var sharding = ClusterSharding.Get(system);

var shardRegion = await sharding.StartAsync(
    typeName: "my-sharding",
    entityPropsFactory: id => Props.Create(() => new ShardingActor(id)),
    settings: ClusterShardingSettings.Create(system),
    messageExtractor: new MessageExtractor(10));

var cluster = Cluster.Get(system);
cluster.RegisterOnMemberUp(() =>
{
    SendMessages(shardRegion);
});

void SendMessages(IActorRef shardRegion)
{
    for (int i = 0; i < 10; i++)
    {
        shardRegion.Tell(new ShardEnvelope(i.ToString(), new Message()));
    }
}

Console.ReadLine();

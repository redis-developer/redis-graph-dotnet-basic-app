using NRedisGraph;
using StackExchange.Redis;

var muxer = ConnectionMultiplexer.Connect("localhost");
var db = muxer.GetDatabase();
var graph = new RedisGraph(db);

// Create Bob
var createBobResult = await graph.QueryAsync("pets", "CREATE(:human{name:'Bob',age:32})");

//Create Alice
await graph.QueryAsync("pets", "CREATE(:human{name:'Alice',age:30})");

//Print out some statistics from the bob 
Console.WriteLine($"Nodes Created:{createBobResult.Statistics.NodesCreated}");
Console.WriteLine($"Properties Set:{createBobResult.Statistics.PropertiesSet}");
Console.WriteLine($"Labels Created:{createBobResult.Statistics.LabelsAdded}");
Console.WriteLine($"Operation took:{createBobResult.Statistics.QueryInternalExecutionTime}");

await graph.QueryAsync("pets", "CREATE(:pet{name:'Honey',age:5,species:'canine',breed:'Greyhound'})");
await graph.QueryAsync("pets",
    "MATCH(a:human),(p:pet) WHERE(a.name='Bob' and p.name='Honey') CREATE (a)-[:OWNS]->(p)");
await graph.QueryAsync("pets",
    "MATCH(a:human),(p:pet) WHERE(a.name='Alice' and p.name='Honey') CREATE (a)-[:WALKS]->(p)");
await graph.QueryAsync("pets",
    "MATCH(a:human),(p:pet) WHERE(a.name='Bob' and p.name='Honey') CREATE (a)-[:WALKS]->(p)");

var matches = await graph.QueryAsync("pets", "MATCH(a:human),(p:pet) where (a)-[:OWNS]->(p) and p.name='Honey' return a");
Console.WriteLine("Honey's owners:");
foreach (var match in matches)
{
    Console.WriteLine(((Node) match.Values.First()).PropertyMap["name"].Value);
}

matches = await graph.QueryAsync("pets", "MATCH(a:human),(p:pet) where (a)-[:WALKS]->(p) and p.name='Honey' return a");
Console.WriteLine("Honey's walkers:");
foreach (var match in matches)
{
    Console.WriteLine(((Node) match.Values.First()).PropertyMap["name"].Value);
}

Console.WriteLine("Dogs owned by Bob:");

matches = await graph.QueryAsync("pets", "MATCH(a:human),(p:pet) where (a)-[:OWNS]->(p) and p.species='canine' and a.name='Bob' return p");
foreach (var match in matches)
{
    Console.WriteLine(((Node) match.Values.First()).PropertyMap["name"].Value);
}

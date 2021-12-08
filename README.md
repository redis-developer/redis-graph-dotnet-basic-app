# Basic RedisGraph .NET Console App

Welcome to this basic example of how to store and query nodes and relationships using [RedisGraph](https://oss.redis.com/redisgraph/). Contained in here is some operable code snippets for using RedisGraph with .NET

## How to Run

To run this app, all you need to do is start Redis with the RedisGraph module activated. The quickest way to do that locally is to use docker:

```bash
docker run -p 6379:6379 redislabs/redisgraph
```

If you are a bit futher a long and run to run RedisGraph in production, the best way to use RedisGraph is to use the [Redis Enterprise Cloud](https://app.redislabs.com/#/)

After you have RedisGraph running, just execute the following from your terminal:

```bash
dotnet run
```

## How it Works

The way this demo works is essentially just using the `QueryAsync` method of the `RedisGraph` class in [NRedisGraph](https://github.com/tombatron/NRedisGraph). Essentially a query applies to a given key where the graph is stored:

```csharp
var createBobResult = await graph.QueryAsync("pets", "CREATE(:human{name:'Bob',age:32})");
```

For example, will create a node with the label `human` on the graph `pets`.

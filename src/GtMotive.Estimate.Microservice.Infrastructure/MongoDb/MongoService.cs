using GtMotive.Estimate.Microservice.Domain.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Domain.MongoDb
{
    public class MongoService(IOptions<MongoDbSettings> options)
    {
        public MongoClient MongoClient { get; } = new MongoClient(options.Value.ConnectionString);
    }
}

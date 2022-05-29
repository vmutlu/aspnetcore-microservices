using Catalog.API.DataAccess.Abstract;
using Catalog.API.DataAccess.SeedDatas;
using Catalog.API.Entities;
using Catalog.API.Settings.Abstract;
using MongoDB.Driver;

namespace Catalog.API.DataAccess.Concrate
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IDatabaseSettings configuration)
        {
            var client = new MongoClient(configuration.ConnectionString);
            var database = client.GetDatabase(configuration.DatabaseName);
            Products = database.GetCollection<Product>(configuration.CollectionName);

            CatalogContextSeed.SeedData(Products);
        }
        
        public IMongoCollection<Product> Products { get; }
    }
}

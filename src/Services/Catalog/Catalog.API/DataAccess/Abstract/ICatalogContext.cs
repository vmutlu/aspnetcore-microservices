using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.DataAccess.Abstract
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}

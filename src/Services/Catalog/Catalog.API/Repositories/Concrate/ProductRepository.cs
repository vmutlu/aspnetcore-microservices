using Catalog.API.DataAccess.Concrate;
using Catalog.API.Entities;
using Catalog.API.Repositories.Abstract;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories.Concrate
{
    public class ProductRepository : IProductRepository
    {
        private readonly  CatalogContext _context;

        public ProductRepository(CatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product).ConfigureAwait(false);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var product = Builders<Product>.Filter.Eq(p => p.Id, id);

            var deleteProduct = await _context.Products.DeleteOneAsync(product).ConfigureAwait(false);
            
            return deleteProduct.IsAcknowledged && deleteProduct.DeletedCount > decimal.Zero;
        }

        public async Task<Product> GetProduct(string id)
        {
            var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await _context.Products.Find(filter).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            var filter = Builders<Product>.Filter.ElemMatch(p => p.Name, name);

            return await _context.Products.Find(filter).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _context.Products.Find(p => true).ToListAsync().ConfigureAwait(false);

            return products;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateProduct = await _context.Products.ReplaceOneAsync(filter: p=> p.Id == product.Id, replacement:product).ConfigureAwait(false);

            return updateProduct.IsAcknowledged && updateProduct.ModifiedCount > decimal.Zero;
        }
    }
}

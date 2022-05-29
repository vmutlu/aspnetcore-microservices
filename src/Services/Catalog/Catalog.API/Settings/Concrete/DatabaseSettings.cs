using Catalog.API.Settings.Abstract;

namespace Catalog.API.Settings.Concrete
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}

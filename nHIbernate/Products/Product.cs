using System.Text.Json.Serialization;

namespace FirmTracker_Server.nHibernate.Products
{
    public class Product
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Price { get; set; }
        public virtual int Type { get; set; } // 0 for service, 1 for goods
        public virtual int Availability { get; set; }
    }
}

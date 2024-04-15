using FluentNHibernate.Mapping;
namespace FirmTracker_Server.nHibernate.Products
{
    public class ProductMapping : ClassMap<Product>
    {
        public ProductMapping()
        {
            Table("Products");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.Price);
            Map(x => x.Type);
            Map(x => x.Availability);
        }
    }
}

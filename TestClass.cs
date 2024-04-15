using FirmTracker_Server.nHibernate; 
using FirmTracker_Server.nHibernate.Products;
using NHibernate;

namespace FirmTracker_Server
{
    public class TestClass
    {
        public void AddTestProduct()
        {
            SessionFactory.Init("Server=localhost;Database=FirmTrackerDB;User Id=sa;Password=Rap45tro2;");

            var product = new nHibernate.Products.Product
            {
                Name = "Test Product2",
                Description = "This is a test product",
                Price = 11.99m,
                Type = 0, // Goods
                Availability = true
            };

            try
            {
                FirmTracker_Server.nHibernate.Products.ProductCRUD crud = new ProductCRUD();
                crud.AddProduct(product);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}

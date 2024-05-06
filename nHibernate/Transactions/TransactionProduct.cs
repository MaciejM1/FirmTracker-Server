using FirmTracker_Server.nHibernate.Products;
using Newtonsoft.Json;
using NSwag.Annotations;

namespace FirmTracker_Server.nHibernate.Transactions
{
    public class TransactionProduct
    {
        public virtual int Id { get; set; }
        [JsonIgnore] 
        [SwaggerIgnore] 
        public virtual int TransactionId { get; set; }
        public virtual Products.Product Product { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal UnitPrice { get; set; }
    }
}

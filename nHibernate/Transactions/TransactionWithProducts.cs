using FirmTracker_Server.nHibernate.Products;
using Newtonsoft.Json;
using NSwag.Annotations;


namespace FirmTracker_Server.nHibernate.Transactions
{
    public class TransactionWithProducts
    {
        public virtual int Id { get; set; }
        public virtual int TransactionId { get; set; }
        public virtual Products.Product Product { get; set; }
        public virtual int Quantity { get; set; }

    }
}

using FirmTracker_Server.nHibernate.Products;
using System.Text.Json.Serialization;

namespace FirmTracker_Server.nHibernate.Transactions
{
    public class Transaction
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }  
        public virtual int EmployeeId { get; set; }
        public virtual IList<Product> Products { get; set; } = new List<Product>();
        public virtual string PaymentType { get; set; }
        public virtual int Discount { get; set; }
        public virtual string Description { get; set; }

        public Transaction()
        {
            Products = new List<Product>();
        }
    }
}

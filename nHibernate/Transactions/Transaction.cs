using FirmTracker_Server.nHibernate.Products;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirmTracker_Server.nHibernate.Transactions
{
    public class Transaction
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual IList<TransactionProduct> TransactionProducts { get; set; } = new List<TransactionProduct>();
        public virtual string PaymentType { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal TotalPrice { get; set; }//=> TransactionProducts.Sum(tp => ((tp.Quantity * tp.UnitPrice)* ((1 - (Discount / 100)))));// (1 - (Discount / 100)));

        public Transaction()
        {
            TransactionProducts = new List<TransactionProduct>();
        }
    }
}

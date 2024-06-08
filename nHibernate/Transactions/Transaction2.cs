using FirmTracker_Server.nHibernate.Products;
using FirmTracker_Server.nHibernate.Reports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirmTracker_Server.nHibernate.Transactions
{
    public class Transaction2
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual IList<TransactionWithProducts> TransactionProducts { get; set; } = new List<TransactionWithProducts>();
        public virtual string PaymentType { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal TotalPrice { get; set; }//=> TransactionProducts.Sum(tp => ((tp.Quantity * tp.UnitPrice)* ((1 - (Discount / 100)))));// (1 - (Discount / 100)));

        public Transaction2()
        {
            TransactionProducts = new List<TransactionWithProducts>();
        }
    }
}

using Net5Api.Abstraction.Model;
using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.Model
{
    public partial class SalesOrderDetail : IEntity
    {
        public int Id { get; set; }
        public int SalesOrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }

        public virtual Product Product { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rasheed_Traders
{
    using System;
    using System.Collections.Generic;
    
    public partial class SaleItem
    {
        public int id { get; set; }
        public int medicineId { get; set; }
        public int tpId { get; set; }
        public int saleId { get; set; }
        public Nullable<int> bonusId { get; set; }
        public string discount { get; set; }
        public Nullable<int> discountAmount { get; set; }
        public int quantity { get; set; }
        public int subTotal { get; set; }
        public int total { get; set; }
        public System.DateTime createdAt { get; set; }
        public Nullable<System.DateTime> updatedAt { get; set; }
        public bool isDeleted { get; set; }
    
        public virtual Bonu Bonu { get; set; }
        public virtual Medicine Medicine { get; set; }
        public virtual Sale Sale { get; set; }
        public virtual TradingPartener TradingPartener { get; set; }
    }
}
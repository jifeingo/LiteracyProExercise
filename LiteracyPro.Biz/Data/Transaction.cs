//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LiteracyPro.Biz.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transaction
    {
        public int TransactionID { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public int CategoryID { get; set; }
        public string PayeeName { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionMemo { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        public virtual Category Category { get; set; }
    }
}
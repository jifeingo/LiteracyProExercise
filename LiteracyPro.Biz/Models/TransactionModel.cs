using System;
using LiteracyPro.Biz.Data;

namespace LiteracyPro.Biz.Models
{
    public class TransactionModel
    {
        public int? TransactionID { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int? CategoryID { get; set; }
        public string CategoryName { get; private set; }
        public string PayeeName { get; set; }
        public decimal? TransactionAmount { get; set; }
        public string TransactionMemo { get; set; }
        public DateTime? CreateDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }
        
        public TransactionModel()
        {
            
        }

        public TransactionModel(Transaction dataRow)
        {
            TransactionID = dataRow.TransactionID;
            TransactionDate = dataRow.TransactionDate;
            TransactionAmount = dataRow.TransactionAmount;
            TransactionMemo = dataRow.TransactionMemo;
            CategoryID = dataRow.CategoryID;
            CategoryName = dataRow.Category.CategoryName;
            PayeeName = dataRow.PayeeName;
            CreateDate = dataRow.CreateDate;
            UpdateDate = dataRow.UpdateDate;
        }
    }
}
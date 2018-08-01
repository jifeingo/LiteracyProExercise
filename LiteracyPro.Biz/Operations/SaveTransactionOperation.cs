using System;
using System.Collections.Generic;
using System.Linq;
using LiteracyPro.Biz.Data;
using LiteracyPro.Biz.Extensions;
using LiteracyPro.Biz.Models;

namespace LiteracyPro.Biz.Operations
{
    public class SaveTransactionOperation : OperationFromJSON<TransactionModel, TransactionModel>
    {
        private Transaction _dbTrx { get; set; }
        private Category _dbCat { get; set; }

        public SaveTransactionOperation(string jsonInputModel) : base(jsonInputModel)
        {
        }

        protected override string DisplayName => "Save Transaction";


        protected override List<ResultMessage> ValidateImpl()
        {
            var msgs = new List<ResultMessage>();

            //Payee Name is required
            if (string.IsNullOrWhiteSpace(InputModel.PayeeName))
            {
                msgs.Add(ResultMessage.Error(OperationSection.Validation,"PayeeName is empty","Payee Name is a required field"));
            }

            //Transaction Amount is required and must be greater than 0
            if (InputModel.TransactionAmount.GetValueOrDefault(0) <= 0)
            {
                msgs.Add(ResultMessage.Error(OperationSection.Validation, "TransactionAmount invalid","Amount is required and must be greater than $0."));
            }

            //Transaction date is required
            if (!InputModel.TransactionDate.HasValue)
            {
                msgs.Add(ResultMessage.Error(OperationSection.Validation, "TransactionDate invalid", "Date is a required field."));
            }

            //Category is required
            if (!InputModel.CategoryID.HasValue)
            {
                msgs.Add(ResultMessage.Error(OperationSection.Validation, "CategoryID id empty", "Category must be selected."));
            }
            else
            {
                _dbCat = DB.Categories.FirstOrDefault(x => x.CategoryID == InputModel.CategoryID.Value);
                if (_dbCat == null)
                {
                    msgs.Add(ResultMessage.Error(OperationSection.Validation, "CategoryID is not valid", "Cannot find Category #{0}.",InputModel.CategoryID));
                }
            }

            //If the transaction ID has been supplied, verify that it exists
            if (InputModel.TransactionID.HasValue)
            {
                _dbTrx = DB.Transactions.FirstOrDefault(x => x.TransactionID == InputModel.TransactionID);
                if (_dbTrx == null)
                {
                    msgs.Add(ResultMessage.Error(OperationSection.Validation, "TransactionID is not valid","Cannot find Transaction {0}",InputModel.TransactionID));
                }
            }

            return msgs;
        }

        protected override List<ResultMessage> ExecuteImpl()
        {
            var msgs = new List<ResultMessage>();

            //If the transaction is new, create the database entity for it.
            if (_dbTrx == null)
            {
                _dbTrx = new Transaction
                {
                    CreateDate = DateTime.Now
                };

                DB.Transactions.Add(_dbTrx);

            }

            _dbTrx.CategoryID = InputModel.CategoryID.Value;
            _dbTrx.Category = _dbCat;
            _dbTrx.PayeeName = InputModel.PayeeName.Trim();
            _dbTrx.TransactionDate = InputModel.TransactionDate.Value;
            _dbTrx.TransactionMemo = InputModel.TransactionMemo.TrimSafe();
            _dbTrx.TransactionAmount = InputModel.TransactionAmount.Value;
            _dbTrx.UpdateDate = DateTime.Now;

            return msgs;
        }


        protected override List<ResultMessage> PostSaveImpl()
        {
            var msgs = base.PostSaveImpl();

            SetOuptutModel(new TransactionModel(_dbTrx));

            return msgs;

        }
    }
}
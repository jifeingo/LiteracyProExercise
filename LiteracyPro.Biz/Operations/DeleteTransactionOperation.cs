using System.Collections.Generic;
using System.Linq;
using LiteracyPro.Biz.Data;

namespace LiteracyPro.Biz.Operations
{
    /// <summary>
    /// Operation responsible for deleting a transaction
    /// </summary>
    public class DeleteTransactionOperation : OperationNoModel
    {
        private int _transactionID { get; set; }

        private Transaction _dbTrx = null;

        public DeleteTransactionOperation(int transactionID)
        {
            _transactionID = transactionID;
        }


        protected override string DisplayName => "Delete Transaction";


        protected override List<ResultMessage> ValidateImpl()
        {

            var msgs = new List<ResultMessage>();

            _dbTrx = DB.Transactions.FirstOrDefault(x => x.TransactionID == _transactionID);
            if (_dbTrx == null)
            {
                msgs.Add(ResultMessage.Error(OperationSection.Validation,"Transaction Not Found","Transaction #{0} could not be found",_transactionID));
            }

            return msgs;
        }

        protected override List<ResultMessage> ExecuteImpl()
        {
            var msgs = new List<ResultMessage>();

            DB.Transactions.Remove(_dbTrx);

            return msgs;
        }
    }
}
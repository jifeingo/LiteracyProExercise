using System.Collections.Generic;
using System.Linq;
using LiteracyPro.Biz.Models;

namespace LiteracyPro.Biz.Operations
{
    /// <summary>
    /// Operation Responsible for returning transations from the database
    /// </summary>
    public class GetTransactionsOperation : Operation<List<TransactionModel>>
    {
        protected override string DisplayName => "Get Transactions";

        protected override List<ResultMessage> ValidateImpl()
        {
            var msgs = new List<ResultMessage>();

            return msgs;
        }

        protected override List<ResultMessage> ExecuteImpl()
        {
            var msgs = new List<ResultMessage>();

            var result = DB.Transactions
                .OrderByDescending(x => x.TransactionDate)
                .ThenByDescending(x => x.TransactionID)
                .ToList()
                .Select(x => new TransactionModel(x))
                .ToList();


            SetOuptutModel(result);

            return msgs;
        }
    }
}
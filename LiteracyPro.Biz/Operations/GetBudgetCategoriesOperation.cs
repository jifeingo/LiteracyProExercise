using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteracyPro.Biz.Models;

namespace LiteracyPro.Biz.Operations
{
    /// <summary>
    /// Responsible fore returning a list of available budget categories
    /// </summary>
    public class GetBudgetCategoriesOperation : Operation<List<CategoryModel>>
    {
        protected override string DisplayName => "Get Categories";

        protected override List<ResultMessage> ValidateImpl()
        {
            var msgs = new List<ResultMessage>();

            return msgs;
        }

        protected override List<ResultMessage> ExecuteImpl()
        {
            var msgs = new List<ResultMessage>();

            var result = DB.Categories
                .OrderBy(x => x.DisplayOrderID)
                .ThenBy(x => x.CategoryName)
                .ToList()
                .Select(x => new CategoryModel(x))
                .ToList();

            SetOuptutModel(result);

            return msgs;
        }
    }
}

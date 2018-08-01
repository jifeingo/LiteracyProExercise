using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using System.Web.Mvc;
using LiteracyPro.Biz.Models;
using LiteracyPro.Biz.Operations;
using Newtonsoft.Json;

namespace LiteracyPro.Web.Controllers
{
    public class BudgetController : Controller
    {
        public BudgetController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpGet]
        public string Categories()
        {
            var oper = new GetBudgetCategoriesOperation();
            var operResult = oper.Execute();

            return JsonConvert.SerializeObject(operResult);     //Do this to avoid date jSON format issues preventing sorting
        }

        [System.Web.Mvc.HttpGet]
        public string Transactions()
        {
            var oper = new GetTransactionsOperation();
            var operResult = oper.Execute();
            return JsonConvert.SerializeObject(operResult); //Do this to avoid date jSON format issues preventing sorting
        }


        [System.Web.Mvc.HttpPost]
        public string SaveTransaction()
        {

            Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();

            var oper = new SaveTransactionOperation(json);
            var operResult = oper.Execute();

            return JsonConvert.SerializeObject(operResult); //Do this to avoid date jSON format issues preventing sorting

        }

        [System.Web.Mvc.HttpGet]
        public string DeleteTransaction(int id)
        {

            var oper = new DeleteTransactionOperation(id);
            var operResult = oper.Execute();

            return JsonConvert.SerializeObject(operResult); //Do this to avoid date jSON format issues preventing sorting

        }


    }
}
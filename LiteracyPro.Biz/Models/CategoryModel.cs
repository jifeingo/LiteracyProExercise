namespace LiteracyPro.Biz.Models
{
    public class CategoryModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int DisplayOrderID { get; set; }

        public CategoryModel()
        {
            
        }

        public CategoryModel(LiteracyPro.Biz.Data.Category dataRow)
        {
            CategoryID = dataRow.CategoryID;
            CategoryName = dataRow.CategoryName;
            DisplayOrderID = dataRow.DisplayOrderID;
        }
    }
}
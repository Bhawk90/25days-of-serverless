using Microsoft.WindowsAzure.Storage.Table;

namespace Foodfest.Models
{
    public class FoodfestItem : TableEntity
    {
        public string FullName { get; set; }

        public string Food { get; set; }

        public double Amount { get; set; }
    }
}
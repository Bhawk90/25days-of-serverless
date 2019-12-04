using System;
using Foodfest.Models;

namespace Foodfest
{
    public static class FoodfestFactory
    {
        public static FoodfestItem GetTableModelFor(FoodfestItemPostBindingModel bindingModel)
        {
            var rowPrefix = new Random().Next(1000, 3999);

            return new FoodfestItem()
            {
                Amount = bindingModel.Amount,
                Food = bindingModel.Food,
                FullName = bindingModel.FullName,
                PartitionKey = "Foods",
                RowKey = $"{rowPrefix}__{bindingModel.FullName.ToUpper().Replace(" ", "_")}_{bindingModel.Food.ToUpper().Replace(" ", "_")}"
            };
        }

        public static FoodfestItemBindingModel GetBindingModelFor(FoodfestItem tableModel)
        {
            return new FoodfestItemBindingModel()
            {
                RowKey = tableModel.RowKey,
                Amount = tableModel.Amount,
                Food = tableModel.Food,
                FullName = tableModel.FullName
            };
        }
    }
}
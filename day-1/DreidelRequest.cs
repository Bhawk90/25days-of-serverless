using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Dreidel.Function
{
    public static class DreidelRequest
    {
        [FunctionName("DreidelRequest")]
        public static DreidelModel Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Dreidel/Spin")] HttpRequest req,
            ILogger log)
        {
            List<DreidelModel> dreidelModels = GetDreidelModels();

            Random rnd = new Random();
            int sideIndex = rnd.Next(1, dreidelModels.Count) - 1;

            return dreidelModels[sideIndex];
        }

        private static List<DreidelModel> GetDreidelModels()
        {
            return new List<DreidelModel>()
            {
                new DreidelModel() {
                    Name = "Nun",
                    Icon = "נ"
                },
                new DreidelModel() {
                    Name = "Gimmel",
                    Icon = "ג"
                },
                new DreidelModel() {
                    Name = "Hay",
                    Icon = "ה"
                },
                new DreidelModel() {
                    Name = "Shin",
                    Icon = "ש"
                }
            };
        }
    }
}

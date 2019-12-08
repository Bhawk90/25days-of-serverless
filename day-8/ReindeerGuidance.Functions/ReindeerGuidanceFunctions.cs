using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.SignalR.Common;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReindeerGuidance.Core;
using ReindeerGuidance.Core.Data;
using ReindeerGuidance.Core.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReindeerGuidance.Functions
{
    public class ReindeerGuidanceFunctions
    {
        private readonly IIncidentRepository _incidentRepository;

        public ReindeerGuidanceFunctions(
            IIncidentRepository incidentRepository
        )
        {
            this._incidentRepository = incidentRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
        }

        [FunctionName("GetStatus")]
        public async Task<IActionResult> GetStatus(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "status")] HttpRequest req,
            ILogger log)
        {
            var incidents = await _incidentRepository.GetAsync();
            foreach (var incident in incidents)
            {
                if (incident.StatusProperty != IncidentEntity.IncidentStatus.Closed.ToString())
                {
                    return new OkObjectResult("incident");
                }
            }

            return new OkObjectResult("ok");
        }

        [FunctionName("GetIncidents")]
        public async Task<IActionResult> GetIncidents(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "incident")] HttpRequest req,
            ILogger log)
        {
            return new OkObjectResult((await _incidentRepository.GetAsync()).OrderByDescending(i => i.ModifiedAt));
        }

        [FunctionName("CreateIncident")]
        public async Task<IActionResult> CreateIncident(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "incident")] IncidentPostBindingModel model,
            [SignalR(HubName = "incidents")]IAsyncCollector<SignalRMessage> signalr,
            ILogger log)
        {
            try
            {
                var incident = await _incidentRepository.CreateAsync(model);

                await signalr.AddAsync(
                    new SignalRMessage
                    {
                        Target = "broadcast",
                        Arguments = new[] { "update" }
                    });

                return new OkObjectResult(incident);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [FunctionName("UpdateIncident")]
        public async Task<IActionResult> UpdateIncident(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "incident/{id}")] IncidentPutBindingModel model,
            [SignalR(HubName = "incidents")]IAsyncCollector<SignalRMessage> signalr,
            ILogger log,
            string id)
        {
            try
            {
                await _incidentRepository.UpdateAsync(id, model);

                await signalr.AddAsync(
                    new SignalRMessage
                    {
                        Target = "broadcast",
                        Arguments = new[] { "update" }
                    });

                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [FunctionName("Subscribe")]
        public SignalRConnectionInfo Subscribe(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "subscribe")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "incidents")]SignalRConnectionInfo connectionInfo,
            ILogger log)
        {
            return connectionInfo;
        }
    }
}

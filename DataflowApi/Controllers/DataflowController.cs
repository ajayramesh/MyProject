using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DataflowApi.Data;
using DataflowApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;
using Newtonsoft.Json;

namespace DataflowApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class DataflowController : ControllerBase
    {
        //private readonly MockDataflowRepo _repository = new MockDataflowRepo();
        private IList<FlowLog> _flowlogCache = new List<FlowLog>();

        [HttpGet("flows")]
        public async Task<IActionResult> ReadFlowlogApiAsync([FromQuery] int? hour)
        {
            IEnumerable<FlowLog> flowlogs = null;
            IEnumerable<FlowLog> result = null;

            try
            {
                // check if hour is null or no hour query filter is provided, if no hour query filter is provided print all flow logs
                if (hour == null)
                {
                    flowlogs = _flowlogCache;     // This can be improved to if flowlogs increase
                    return await Task.FromResult(Ok(flowlogs));
                }

                flowlogs = _flowlogCache;

                //check if there are logs available else return 404
                IEnumerable<FlowLog> flowLogs = flowlogs.ToList();
                if (!flowLogs.Any())
                {
                    return await Task.FromResult(StatusCode(404, "No logs found"));
                }

                // filter the logs based on query filter and group them based on SrcApp, DestApp, VpcId, Hour
                result = (from each in flowLogs
                    where each.Hour == hour
                    group each by new {each.SrcApp, each.DestApp, each.VpcId, each.Hour}
                    into eg
                    select new FlowLog
                    {
                        SrcApp = eg.Key.SrcApp,
                        DestApp = eg.Key.DestApp,
                        VpcId = eg.Key.VpcId,
                        BytesTx = eg.Sum(s => s.BytesTx),
                        BytesRx = eg.Sum(s => s.BytesRx),
                        Hour = eg.Key.Hour,
                    });
            }
            catch (Exception ex)
            {
                // Internal server error - There was some error to retrieve the data
                return StatusCode(500, "Error occurred to get the flow logs");
            }

            return Ok(result);
        }

        [HttpPost("flows")]
        public async Task<IActionResult> WriteFlowlogAsync([FromBody] object flowlogs)
        {
            List<FlowLog> postedFlowlogObjectList = new List<FlowLog>();

            if (flowlogs == null)
            {
                return await Task.FromResult(StatusCode(400, "Bad Request (POST)"));
            }

            try
            {
                var postedFlowlogs = JsonConvert.DeserializeObject<List<FlowLog>>(flowlogs.ToString());
                foreach (var eachPostedFlowlog in postedFlowlogs)
                {
                    FlowLog flowLogObject = new FlowLog
                    {
                        SrcApp = eachPostedFlowlog.SrcApp,
                        DestApp = eachPostedFlowlog.DestApp,
                        VpcId = eachPostedFlowlog.VpcId,
                        BytesRx = eachPostedFlowlog.BytesRx,
                        BytesTx = eachPostedFlowlog.BytesTx,
                        Hour = eachPostedFlowlog.Hour
                    };
                    postedFlowlogObjectList.Add(flowLogObject);
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(500, $"Internal Server error (POST) , with exception {ex}"));
            }

            _flowlogCache = postedFlowlogObjectList.ToList();
            return await Task.FromResult(Ok(_flowlogCache));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataflowApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataflowApi.Data
{
    public class MockDataflowRepo : IDataflowRepo
    {
        public IEnumerable<FlowLog> GetFlowData()
        {
            var flowlogs = new List<FlowLog>
            {
                new FlowLog {SrcApp = "SourceApp1", DestApp = "DestinationApp1", VpcId = "vpc-0", BytesTx = 100, BytesRx = 300, Hour = 1},
                new FlowLog {SrcApp = "SourceApp1", DestApp = "DestinationApp1", VpcId = "vpc-0", BytesTx = 200, BytesRx = 600, Hour = 1},
                new FlowLog {SrcApp = "SourceApp2", DestApp = "DestinationApp2", VpcId = "vpc-0", BytesTx = 100, BytesRx = 500, Hour = 1},
                new FlowLog {SrcApp = "SourceApp2", DestApp = "DestinationApp2", VpcId = "vpc-0", BytesTx = 100, BytesRx = 500, Hour = 2},
                new FlowLog {SrcApp = "SourceApp2", DestApp = "DestinationApp2", VpcId = "vpc-1", BytesTx = 100, BytesRx = 500, Hour = 2}
            };

            return flowlogs;
        }

        public FlowLog GetPayloadBySrcAndDest(string src, string dest)
        {
            return new FlowLog
            {
                SrcApp = "SourceApp",
                DestApp = "DestinationApp",
                BytesRx = 0,
                BytesTx = 0,
                Hour = 1
            };
        }
    }
}

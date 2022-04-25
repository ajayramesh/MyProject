using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataflowApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataflowApi.Data
{
    public interface IDataflowRepo
    {
        //Read Api
        IEnumerable<FlowLog> GetFlowData();
        FlowLog GetPayloadBySrcAndDest(string src, string dest);




    }
}

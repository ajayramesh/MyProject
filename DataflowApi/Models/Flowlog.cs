using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataflowApi.Models
{
    // Flowlog class representing the data model (Network payload)
    public class FlowLog
    {
        //Source App
        public string SrcApp { get; set; }
        
        //Destination App
        public string DestApp { get; set; }

        //Id of the VPC 
        public string VpcId { get; set; }

        // Bytes Transferred
        public int BytesTx { get; set; }

        //Bytes Received
        public int BytesRx { get; set; }

        // Hour the network flow was logged
        public int? Hour { get; set; }
    }
}

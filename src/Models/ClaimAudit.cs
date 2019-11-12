using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimHandlingApp.Models
{
    public class ClaimAudit
    {
        [JsonProperty(PropertyName = "claimid")]
        public string ClaimId { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public string TimeStamp { get; set; }

        [JsonProperty(PropertyName = "operation")]
        public string Operation { get; set; }
    }
}

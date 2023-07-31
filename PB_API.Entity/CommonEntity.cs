using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PB_API.Entity
{
    //Added by Yatin on 31/07/2023
    public class CommonEntity
    {
        public bool IsDownload { get; set; }

    }

    public class CommonSqlLiteEntity
    {
        [JsonProperty("0")]
        public string Data { get; set; }
    }
    //End by Yatin

}

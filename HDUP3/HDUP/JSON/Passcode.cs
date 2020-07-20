using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HDUP3.HDUP.JSON
{
    class Passcode
    {

        [JsonProperty("Date")]
        public String Date { get; set; }

        [JsonProperty("Code")]
        public int Code { get; set; }

    }
}

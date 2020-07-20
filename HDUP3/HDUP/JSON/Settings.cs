using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDUP3.HDUP.JSON
{
    class Settings
    {

        // Data file locations

        [JsonProperty("StoreJsonLocation")]
        public string StoreJsonLocation { get; set; }

        [JsonProperty("DeviceJsonLocation")]
        public string DeviceJsonLocation { get; set; }

        [JsonProperty("ActionJsonLocation")]
        public string ActionJsonLocation { get; set; }

        [JsonProperty("PasscodeJsonLocation")]
        public string PasscodeJsonLocation { get; set; }

        [JsonProperty("ScriptFolderLocation")]
        public string ScriptFolderLocation { get; set; }

        // Logs/Errors

        [JsonProperty("LogsTextLocation")]
        public string LogsTextLocation { get; set; }

        [JsonProperty("ErrorsTextLocation")]
        public string ErrorsTextLocation { get; set; }

        // SQL server information

        [JsonProperty("SQL_Username")]
        public string SQL_Username { get; set; }

        [JsonProperty("SQL_Password")]
        public string SQL_Password { get; set; }

        // Remotely Anywhere information

        [JsonProperty("Remotely_Username")]
        public string Remotely_Username { get; set; }

        [JsonProperty("Remotely_Password")]
        public string Remotely_Password { get; set; }

        [JsonProperty("Remotely_Browser")]
        public string Remotely_Browser { get; set; }
    }
}

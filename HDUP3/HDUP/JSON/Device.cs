using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace HDUP3.HDUP.JSON
{
    class Device
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("IP-Format")]
        public String IpFormat { get; set; }

        [JsonProperty("Actions")]
        public String Actions { get; set; }

        [JsonProperty("Name")]
        public String Name { get; set; }

        public String toString()
        {
            return "Device:: ID=" + ID + " Name=" + Name + " IP-Format=" + IpFormat + " Actions=" + Actions;
        }

        public List<int> getActionsID()
        {
            List<int> actionsID = new List<int>();

            if (Actions.Contains(","))
            {
                String[] actions = Actions.Split(',');

                foreach (String action in actions)
                {
                    actionsID.Add(int.Parse(action));
                }
            }
            else
                actionsID.Add(int.Parse(Actions));

            return actionsID;
        }

        public string getDomain(int StoreID, Boolean Relocation)
        {
            IPAddress address = IPAddress.Parse(getIPForStore(StoreID, Relocation));
            IPHostEntry entry = Dns.GetHostEntry(address);

            return entry.HostName.Split('.')[0];
        }

        public string getDomain(int StoreID)
        {
            return getDomain(StoreID, false);
        }

        public Boolean IsIPv6()
        {
            return !IpFormat.Contains(".");
        }

        public String getIPForStore(int StoreID, Boolean Relocation)
        {
            if (!IsIPv6())
            {
                String fourDigitStoreNumber = StoreID.ToString("D" + 4);
                String strIpFormat = "";
                String IP = "";

                int count = 0;

                if (Relocation)
                {
                    String[] aryIpFormat = IpFormat.Split('.');

                    for (int i = 0; i < aryIpFormat.Length; i++)
                    {
                        if (i == 1)
                            strIpFormat += "." + aryIpFormat[i].Replace("1", "2");
                        else if (i == 0)
                            strIpFormat += aryIpFormat[i];
                        else
                            strIpFormat += "." + aryIpFormat[i];
                    }
                }
                else
                    strIpFormat = IpFormat;

                foreach (char x in strIpFormat)
                {
                    if (x.ToString().ToLower().Equals("x"))
                    {
                        IP += fourDigitStoreNumber[count];
                        count++;
                    }
                    else
                        IP += x;
                }

                return IP;
            }

            return "";
        }

        public String getIPForStore(int StoreID)
        {
            return getIPForStore(StoreID, false);
        }
    }
}

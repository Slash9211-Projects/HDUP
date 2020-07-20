using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDUP3.HDUP.JSON
{
    class Store
    {
        public int ID { get; set; }

        public String Devices { get; set; }

        public Boolean Relocation { get; set; }

        public String toString()
        {
            return "Store:: ID=" + ID + " Relocation=" + Relocation + " Devices=" + Devices;
        }

        public List<int> getDevicesID()
        {
            List<int> devicesID = new List<int>();

            String[] devices = Devices.Split(',');

            foreach (String device in devices)
            {
                devicesID.Add(int.Parse(device));
            }

            return devicesID;
        }
    }
}

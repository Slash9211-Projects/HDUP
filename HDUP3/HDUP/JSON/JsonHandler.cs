using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace HDUP3.HDUP.JSON
{
    class JsonHandler
    {

        public static List<Store> stores;
        public static List<Device> devices;
        public static List<Actions> actions;

        public static int GetPasscode()
        {
            List<Passcode> Codes = (List<Passcode>)Desteralize(MainWindow.PasscodeJsonLocation);

            DateTime PreviousDate = Convert.ToDateTime("01/01/2000");
            DateTime Today = DateTime.Today;

            int Code = 0;

            foreach (Passcode ITPasscode in Codes)
            {
                DateTime Date = Convert.ToDateTime(ITPasscode.Date);

                if (Today.DayOfYear >= PreviousDate.DayOfYear && Today.DayOfYear < Date.DayOfYear && Today.Year == Date.Year)
                {
                    return Code;
                }

                PreviousDate = Date;
                Code = ITPasscode.Code;
            }

            return 0;
        }

        public static Store GetStore(int ID, Boolean relocation)
        {
            foreach (Store store in stores)
            {
                if (store.ID == ID && store.Relocation == relocation)
                    return store;
            }

            return null;
        }

        public static Device GetDevice(int ID)
        {
            foreach (Device device in devices)
            {
                if (device.ID == ID)
                    return device;
            }

            return null;
        }

        public static Actions GetAction(int ID)
        {
            foreach (Actions action in actions)
            {
                if (action.ID == ID)
                    return action;
            }

            return null;
        }

        private static String GetFileName(string FileLocation)
        {
            string[] X = FileLocation.Split('/');

            foreach (String Y in X)
            {
                if (Y.Contains("."))
                    return Y.Split('.')[0];
            }

            return "null";
        }

        public static String GetJsonText(String FileLocation)
        {
            return File.ReadAllText(FileLocation);
        }

        public static String Steralize(Object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static Object Desteralize(String FileLocation)
        {
            string Type = GetFileName(FileLocation).ToLower();
            string JSON = GetJsonText(FileLocation);

            if (Type.Contains("store"))
            {
                return JsonConvert.DeserializeObject<List<Store>>(JSON);
            }
            else if (Type.Contains("device"))
            {
                return JsonConvert.DeserializeObject<List<Device>>(JSON);
            }
            else if (Type.Contains("action"))
            {
                return JsonConvert.DeserializeObject<List<Actions>>(JSON);
            }
            else if (Type.Contains("settings"))
            {
                return JsonConvert.DeserializeObject<Settings>(JSON);
            }
            else if (Type.Contains("passcode"))
            {
                return JsonConvert.DeserializeObject<List<Passcode>>(JSON);
            }

            return null;
        }

        public static void Save(String JSON, String FileLocation)
        {
            File.WriteAllText(FileLocation, JSON);
        }

        public static void Save(Object obj, String FileLocation)
        {
            if (obj.GetType().Equals(typeof(List<Store>)))
            {
                List<Store> store = (List<Store>)obj;

                store.Sort(new StoreSorter());

                obj = store;
            }
            else if (obj.GetType().Equals(typeof(List<Device>)))
            {
                List<Device> device = (List<Device>)obj;

                device.Sort(new DeviceSorter());

                obj = device;
            }
            else if (obj.GetType().Equals(typeof(List<Actions>)))
            {
                List<Actions> action = (List<Actions>)obj;

                action.Sort(new ActionSorter());

                obj = action;
            }

            File.WriteAllText(FileLocation, Steralize(obj));
        }

        // Sorts from least to greatest numbers
        private class StoreSorter : IComparer<Store>
        {
            public int Compare(Store x, Store y)
            {
                if (x == null || y == null)
                {
                    return 0;
                }

                return x.ID.CompareTo(y.ID);
            }
        }

        private class DeviceSorter : IComparer<Device>
        {
            public int Compare(Device x, Device y)
            {
                if (x == null || y == null)
                {
                    return 0;
                }

                return x.ID.CompareTo(y.ID);
            }
        }

        private class ActionSorter : IComparer<Actions>
        {
            public int Compare(Actions x, Actions y)
            {
                if (x == null || y == null)
                {
                    return 0;
                }
                
                return x.ID.CompareTo(y.ID);
            }
        }
    }
}

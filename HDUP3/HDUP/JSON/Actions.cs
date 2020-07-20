using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace HDUP3.HDUP.JSON
{
    class Actions
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Code")]
        public List<string> Code { get; set; }

        [JsonProperty("PremadeLocation")]
        public string PremadeLocation { get; set; }

        [JsonProperty("Local")]
        public Boolean Local { get; set; }

        private String FolderLocation = MainWindow.ScriptFolderLocation;

        private String ReplaceVariables(String line, int StoreID, int DeviceID, Boolean Relocation)
        {
            line = line.Replace("%IP%", JsonHandler.GetDevice(DeviceID).getIPForStore(StoreID, Relocation));
            line = line.Replace("%RA-USERNAME%", MainWindow.RA_Username);
            line = line.Replace("%RA-PASSWORD%", MainWindow.RA_Password);
            line = line.Replace("%DOMAIN%", JsonHandler.GetDevice(DeviceID).getDomain(StoreID, Relocation));

            return line;
        }

        private void RemotelyAnywhere(String type, int StoreID, Boolean Relocation, int DeviceID)
        {
            Device device = JsonHandler.GetDevice(DeviceID);

            ProcessStartInfo startInfo = new ProcessStartInfo();

            String IpAddress = device.getIPForStore(StoreID, Relocation);

            String domain = device.getDomain(StoreID, Relocation);

            startInfo.Arguments = type + " -computer " + IpAddress + " -login " + MainWindow.RA_Username + " -pass " + MainWindow.RA_Password;
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = @"c:\Program Files (x86)\Network Console\";
            startInfo.FileName = "networkconsole.exe";

            try
            {
                Process.Start(startInfo);
            } 
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Failed to start Network Console: " + ex);
            }
        }

        public void Run(int StoreID, Boolean relocation, int DeviceID)
        {
            if (PremadeLocation == null)
            {
                if (Type.ToLower().Equals("sql")) // TODO Test
                {
                    foreach (String Command in Code)
                    {
                        SQL.SqlHandler.RunCommand(StoreID, DeviceID, Command);
                    }
                }  
                //                             Remote Desktop                               FTP GUI                            Control Panel
                else if (Type.ToLower().Equals("remotecontrol") || Type.ToLower().Equals("filetransfer") || Type.ToLower().Equals("openra"))
                {
                    RemotelyAnywhere(Type.ToLower(), StoreID, relocation, DeviceID);
                }
                else if (Type.ToLower().Equals("browser"))
                {
                    foreach (String line in Code)
                    {
                        try {
                            Process.Start(MainWindow.RA_Browser, ReplaceVariables(line, StoreID, DeviceID, relocation));
                        }
                        catch (Win32Exception e)
                        {
                            var psi = new ProcessStartInfo();
                            psi.UseShellExecute = true;
                            psi.FileName = ReplaceVariables(line, StoreID, DeviceID, relocation);
                            Process.Start(psi);
                        }
                        
                    }
                }
                else if (Type.ToLower().Equals("ftp"))
                {
                    String IP = JsonHandler.GetDevice(DeviceID).getIPForStore(StoreID);

                    foreach (String FTP in Code)
                    {
                        String[] Files = ReplaceVariables(FTP, StoreID, DeviceID, relocation).Split('>');

                        File.Copy(@"" + Files[0], @"" + Files[1], true);
                    }
                }
                else if (Type.ToLower().Equals("run"))
                {
                    foreach (String Application in Code)
                    {
                        System.Diagnostics.Process.Start(Application);
                    }
                }
                else if (Type.ToLower().Equals("shell") || Type.ToLower().Equals("batch"))
                {
                    String extension = "";

                    if (Type.ToLower().Equals("shell"))
                        extension = "sh";
                    else
                        extension = "bat";

                    List<String> FinalCode = new List<String>();

                    foreach (String line in Code)
                    {
                        FinalCode.Add(ReplaceVariables(line, StoreID, DeviceID, relocation));
                    }

                    Code.Add("del \" % ~f0\" & exit"); // Adding code to the script to automatically delete itself on completion

                    File.WriteAllLines(FolderLocation + Name + "." + extension, FinalCode);

                    if (Local == true) // Local
                        System.Diagnostics.Process.Start(FolderLocation + Name + "." + extension);
                    else // Remotely
                    {
                        String IP = JsonHandler.GetDevice(DeviceID).getIPForStore(StoreID, relocation);
                        File.Copy(FolderLocation + Name + "." + extension, @"\\" + IP + "\\c-drive\\QTreg\\RunOnce\\" + Name + "." + extension, true);
                    }

                }
            }
            else
            {
                if (Local == true)
                    System.Diagnostics.Process.Start(PremadeLocation);
                else
                {
                    String IP = JsonHandler.GetDevice(DeviceID).getIPForStore(StoreID, relocation);
                    File.Copy(PremadeLocation, @"\\" + IP + "\\c-drive\\QTreg\\RunOnce\\" + Name + "." + PremadeLocation.Split('.')[1], true);
                }
            }
        }
    }
}

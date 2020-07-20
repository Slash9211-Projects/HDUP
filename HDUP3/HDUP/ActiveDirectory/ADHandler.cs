using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HDUP3.HDUP.ActiveDirectory
{
    class ADHandler
    {
        public static List<String> GetUsersGroups()
        {
            List<String> Groups = new List<String>();
            WindowsIdentity windowsIdentity = new WindowsIdentity("USER"); // Replace USER with the users actual username, find a way to automatically do this

            foreach (IdentityReference Group in windowsIdentity.Groups)
            {
                try
                {
                    Groups.Add(Group.Translate(typeof(NTAccount)).ToString());
                }
                catch (Exception ex) { }
            }

            return Groups;
        }
    }
}

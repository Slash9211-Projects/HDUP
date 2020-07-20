using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDUP3.HDUP.SQL
{
    class SqlHandler
    {
        //Devices may have their own server located on them, will have to talk to Bill on that
        // TODO Look further into this to see how it'd operate and test at home before hand
        public static string RunCommand(int StoreID, int DeviceID, String strCommand)
        {
            String Output = "";
            String strConnection = "";
            SqlConnection Connection;

            //strConnection = @"Data Source=" + MainWindow.SQL_Server + ";Initial Catalog=" + MainWindow.SQL_Database + ";User ID=" + MainWindow.SQL_Username + ";Password=" + MainWindow.SQL_Password;

            Connection = new SqlConnection(strConnection);

            Connection.Open();

            SqlCommand Command;
            SqlDataReader DataReader;

            Command = new SqlCommand(strCommand, Connection);

            DataReader = Command.ExecuteReader();

            while (DataReader.Read())
            {
                Output += DataReader.GetValue(0) + " - " + DataReader.GetValue(1) + "\n";
            }

            DataReader.Close();
            Command.Dispose();
            Connection.Close();

            return Output;
        }
    }
}

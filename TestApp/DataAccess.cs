using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace SynthesisDataToCSV
{
    public class DataAccess
    {
        public DataTable GetData()
        {

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string MarysvilleEndingInvQuery = string.Empty;

            using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("SynthesisDataToCSV.MarysvilleEndingInv_query.txt"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    MarysvilleEndingInvQuery = reader.ReadToEnd();
                }
            }
            DateTime startDt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endDt = startDt.AddMonths(1).AddDays(-1);

            using (SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand(MarysvilleEndingInvQuery, sqlcon))
                {
                    sqlcon.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Start_Date", startDt);
                    cmd.Parameters.AddWithValue("@End_Date", endDt);
                    SqlDataAdapter dbAdaptor = new SqlDataAdapter(cmd);
                    dbAdaptor.Fill(ds);
                    dt = ds.Tables[0];


                }
            }

            return dt;
        }
    }
}

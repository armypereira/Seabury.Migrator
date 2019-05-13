using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seabury.Migrator.SqlRepository
{
    public class BaseRepository<T> where T : class
    {
        public bool Load(DataTable valInfo)
        {
            bool vResult = false;
            return vResult;
        }

        public DataSet ExecuteQuery(string valQuery, List<SqlParameter> valParameters, string valConnString)
        {
            DataSet vResult = null;
            try
            {

                SqlConnectionStringBuilder vConStringBuilder = new SqlConnectionStringBuilder();
                string vConnString = string.Empty;
                SqlConnection vSqlCon = new SqlConnection();
                SqlDataAdapter vAdapter = new SqlDataAdapter();
                vConnString = valConnString;
                vSqlCon = new SqlConnection(vConnString);
                List<SqlParameter> vParametrosSQL = new List<SqlParameter>();
                vParametrosSQL = valParameters;
                vAdapter.SelectCommand = new SqlCommand(valQuery, vSqlCon);
                foreach (SqlParameter vSQLParam in vParametrosSQL)
                {
                    vAdapter.SelectCommand.Parameters.Add(vSQLParam);
                }
                vSqlCon.Open();
                vResult = new DataSet();
                vAdapter.Fill(vResult);
                vSqlCon.Close();

            }
#pragma warning disable CS0168 // The variable 'vEx' is declared but never used
            catch (Exception vEx)
#pragma warning restore CS0168 // The variable 'vEx' is declared but never used
            {
                throw;
            }
            return vResult;
        }

        public int CountRow(string valTablename, string  valConnString)
        {
            string vSql = "SELECT COUNT(*) FROM "+ valTablename;
            int  vResult = 0;

            using (SqlConnection vConnection = new SqlConnection(valConnString))
            {
                using (SqlCommand cmdCount = new SqlCommand(vSql, vConnection))
                {
                    vConnection.Open();
                    vResult = (int)cmdCount.ExecuteScalar();
                }
            }
            return vResult;
        }

        public   void PerformBulkCopyXMLDataSource(string valConnString, string valDirectory)
        {
            string vConnectionString = valConnString;
            DataSet ds = new DataSet();
            DataTable vSourceData = new DataTable();
            ds.ReadXml(valDirectory);
            vSourceData = ds.Tables[0];
            vSourceData.Columns.Remove("row_id");
            vSourceData.Columns.Remove("Sys_TimeStamp");

            
            // open the destination data
            using (SqlConnection vDestinationConnection =
                            new SqlConnection(vConnectionString))
            {
                // open the connection
                vDestinationConnection.Open();
                using (SqlBulkCopy vBulkCopy = new SqlBulkCopy(vDestinationConnection.ConnectionString))
                {
                    vBulkCopy.DestinationTableName = vSourceData.TableName+ "Temp";
                    vBulkCopy.BatchSize = vSourceData.Rows.Count;
                    vBulkCopy.NotifyAfter = 1000;
                    vBulkCopy.WriteToServer(vSourceData);
                }
            }
        }
        

    }
}

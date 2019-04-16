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
            catch (Exception vEx)
            {
                throw;
            }
            return vResult;
        }

    }
}

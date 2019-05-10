using Seabury.Migrator.Contract;
using Seabury.Migrator.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seabury.Migrator.SqlRepository
{
    public class DataInfoRepository : BaseRepository<DataInfo>, IDataInfoRepository
    {
        bool IDataInfoRepository.ExportToXML(List<string> valListColumns, string valTableName, int valSizePerPage, string valDirectory)
        {
            bool vResult = true;
            string vSql = SqlExportToXML(valListColumns, valTableName);
            string vConnectionStrings = ConfigurationManager.ConnectionStrings["DBSqlConnection"].ToString();
            int vNP = NumberOfPages(valTableName, valSizePerPage);
            int vStart = 1;
            int vFinish = valSizePerPage;
            for (int i = 0; i < vNP; i++)
            {
                List<SqlParameter> vParametrosSQL = new List<SqlParameter>();
                vParametrosSQL.Add(new SqlParameter("@Start", vStart));
                vParametrosSQL.Add(new SqlParameter("@Finish", vFinish));
                DataSet vDataSet = ExecuteQuery(vSql, vParametrosSQL, vConnectionStrings);
                vStart = valSizePerPage + 1;
                vFinish = valSizePerPage + vFinish;
                Guid vG;
                vG = Guid.NewGuid();
                vDataSet.WriteXml(vG.ToString() + valDirectory);
            }
            return vResult;
        }

        

        string SqlExportToXML(List<string> valListColumns, string valTableName)
        {
            string vResult = string.Empty;
            StringBuilder vSql = new StringBuilder();
            vSql.AppendLine("WITH data");
            vSql.AppendLine("AS");
            vSql.AppendLine("(");
            vSql.AppendLine(GetSqlExportToXMLSelect(valListColumns));
            vSql.AppendLine("FROM ");
            vSql.AppendLine(valTableName);
            vSql.AppendLine(") ");
            vSql.AppendLine(" SELECT * ");
            vSql.AppendLine(" FROM data ");
            vSql.AppendLine(" WHERE row_id BETWEEN @Start AND @Finish ");
            vResult = vSql.ToString();
            return vResult;
        }

        string GetSqlExportToXMLSelect(List<string> valListColumns)
        {
            string vResult = string.Empty;
            bool vFirstColumn = true;
            vResult = "SELECT ROW_NUMBER() OVER (ORDER BY COLUMNSORDER) AS row_id ";
            vResult = vResult.Replace("COLUMNSORDER", valListColumns[0]);
            StringBuilder vSql = new StringBuilder();
            vSql.AppendLine(vResult);
            foreach (var vColumns in valListColumns)
            {
                if (vFirstColumn)
                {

                    vFirstColumn = false;
                }
                else
                {
                    vSql.AppendLine(", " + vColumns);
                }
            }
            vResult = vSql.ToString();
            return vResult;
        }

        int NumberOfPages(string valTableName, int valSizePerPage)
        {
            //Necesito buscar contra la tabla 
            int vResult = 0;
            string vConnectionStrings = ConfigurationManager.ConnectionStrings["DBSqlConnection"].ToString();
            vResult = CountRow(valTableName, vConnectionStrings);
            vResult = (int)(vResult / valSizePerPage);
            vResult = vResult + 1;
            return vResult;

        }

        bool IDataInfoRepository.ExportReportByListToXML(List<string> valListReports, int valSizePerPage, string valDirectory)
        {
            bool vResult = true;
            SqlTool insSqlTool = new SqlTool();
            string vSql = "";
            string vConnectionStrings = ConfigurationManager.ConnectionStrings["DBSqlConnection"].ToString();
            List<string> vReportSqlList = ReportSqlList(valListReports);
            foreach (var vRecord in vReportSqlList)
            {
                vSql = vRecord;
                int vNP = 5000;
                int vStart = 1;
                int vFinish = valSizePerPage;

                for (int i = 0; i < vNP; i++)
                {
                    List<SqlParameter> vParametrosSQL = new List<SqlParameter>();
                    vParametrosSQL.Add(new SqlParameter("@Start", vStart));
                    vParametrosSQL.Add(new SqlParameter("@Finish", vFinish));
                    DataSet vDataSet = ExecuteQuery(vSql, vParametrosSQL, vConnectionStrings);
                    vStart = valSizePerPage + 1;
                    vFinish = valSizePerPage + vFinish;
                    Guid vG;
                    vG = Guid.NewGuid();
                    vDataSet.WriteXml(vG.ToString() + valDirectory);
                }
            }
            return vResult;
        }

        List<string> ReportSqlList(List<string> valListReports)
        {
            //aca buscamos N reportes, con el fin de exportar cada reporte , esto aca armaria una lista de sql para la exportacion con JOIN a las vistas base para poder filtrar por el id del sql
            List<string> vResult = new List<string>();
            SqlTool insSqlTool = new SqlTool();
            vResult.Add(insSqlTool.SqlQueriesExpor(valListReports));
            return vResult;

        }

        bool IDataInfoRepository.ExportToXML(Dictionary<string, string> valTableNameAndPK, int valSizePerPage, string valDirectory)
        {
            bool vResult = true;
            string vConnectionStrings = ConfigurationManager.ConnectionStrings["DBSqlConnection"].ToString();
            foreach (KeyValuePair<string, string> kvp in valTableNameAndPK.AsParallel())
            {
                string vSql = SqlExportToXML(kvp.Value, kvp.Key);
                int vNP = NumberOfPages(kvp.Value, valSizePerPage);
                int vStart = 1;
                int vFinish = valSizePerPage;
                for (int i = 0; i < vNP; i++)
                {
                    List<SqlParameter> vParametrosSQL = new List<SqlParameter>();
                    vParametrosSQL.Add(new SqlParameter("@Start", vStart));
                    vParametrosSQL.Add(new SqlParameter("@Finish", vFinish));
                    DataSet vDataSet = ExecuteQuery(vSql, vParametrosSQL, vConnectionStrings);
                    vStart = valSizePerPage + 1;
                    vFinish = valSizePerPage + vFinish;
                    Guid vG;
                    vG = Guid.NewGuid();
                    vDataSet.WriteXml(valDirectory+ "\\"+kvp.Value + vG.ToString());
                }
            }
            return vResult;
        }


        string GetSqlExportToXMLSelect(string valPK)
        {
            string vResult = string.Empty;
            vResult = "SELECT ROW_NUMBER() OVER (ORDER BY COLUMNSORDER) AS row_id ";
            vResult = vResult.Replace("COLUMNSORDER", valPK);
            StringBuilder vSql = new StringBuilder();
            vSql.AppendLine(vResult);
            vSql.AppendLine(",*");
            vResult = vSql.ToString();
            return vResult;
        }

        string SqlExportToXML(string valTableName, string valPK)
        {
            string vResult = string.Empty;
            StringBuilder vSql = new StringBuilder();
            vSql.AppendLine("WITH data");
            vSql.AppendLine("AS");
            vSql.AppendLine("(");
            vSql.AppendLine(GetSqlExportToXMLSelect(valPK));
            vSql.AppendLine("FROM ");
            vSql.AppendLine(valTableName);
            vSql.AppendLine(") ");
            vSql.AppendLine(" SELECT * ");
            vSql.AppendLine(" FROM data ");
            vSql.AppendLine(" WHERE row_id BETWEEN @Start AND @Finish ");
            vResult = vSql.ToString();
            return vResult;
        }


        bool IDataInfoRepository.Load(DataInfo valInfo)
        {
            throw new NotImplementedException();
        }
    }
}

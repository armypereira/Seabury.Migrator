using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seabury.Migrator.SqlRepository
{
    public class SqlTool
    {
       public string SqlQueriesExpor(List<string> valListReports)
        {
            string vResult = string.Empty;
            StringBuilder vSql = new StringBuilder();
            string vSqlSelect = SqlQueriesSelectExpor(valListReports);
            vResult = SqlExportBase(vSqlSelect);
            return vResult;

        }
        public string SqlQueriesSelectExpor(List<string> valListReports)
        {
            string vResult = string.Empty;
            StringBuilder vSql = new StringBuilder();
            vSql.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY dbo.ssQueries.QueryID) AS row_id, ");
            vSql.AppendLine("dbo.ssQueries.QueryID, ");
            vSql.AppendLine("dbo.ssQueries.Name, ");
            vSql.AppendLine("dbo.ssQueries.Description, ");
            vSql.AppendLine("dbo.ssQueries.DriveFolderID, ");
            vSql.AppendLine("dbo.ssQueries.QueryDefID, ");
            vSql.AppendLine("dbo.ssQueries.OwnerUserID, ");
            vSql.AppendLine("dbo.ssQueries.UseGroupBy, ");
            vSql.AppendLine("dbo.ssQueries.UseDistinct, ");
            vSql.AppendLine("dbo.ssQueries.xTop, ");
            vSql.AppendLine("dbo.ssQueries.xPercent, ");
            vSql.AppendLine("dbo.ssQueries.xTimeout, ");
            vSql.AppendLine("dbo.ssQueries.Temporal, ");
            vSql.AppendLine("dbo.ssQueries.CustomData, ");
            vSql.AppendLine("dbo.ssQueries.InternalOfSystem, ");
            vSql.AppendLine("dbo.ssQueries.OwnerCompany, ");
            vSql.AppendLine("dbo.ssQueries.SharedCompanies, ");
            vSql.AppendLine("dbo.ssQueries.Sys_Entry_Date, ");
            vSql.AppendLine("dbo.ssQueries.Sys_Entry_User, ");
            vSql.AppendLine("dbo.ssQueries.Sys_Edit_Date, ");
            vSql.AppendLine("dbo.ssQueries.Sys_Edit_User ");
            vSql.AppendLine("FROM    dbo.ssVwReports_B1 ");
            vSql.AppendLine("INNER JOIN   dbo.ssQueries ON ");
            vSql.AppendLine("dbo.ssVwReports_B1.QueryID = dbo.ssQueries.QueryID ");
            vSql.AppendLine("WHERE  dbo.ssVwReports_B1.ReportID IN(" + FilterIn(valListReports) + ") ");
            vResult = vSql.ToString();
            return vResult;
        }

        string FilterIn(List<string> valListReports)
        {
            string vResult = string.Empty;
            foreach (string item in valListReports)
            {
                vResult += item + ",";

            }
            vResult = vResult.Remove(vResult.LastIndexOf(","), 1); //Remove the last comma  
            return vResult;
        }

        string SqlExportBase(string valSelect)
        {
            string vResult = string.Empty;
            StringBuilder vSql = new StringBuilder();
            vSql.AppendLine("WITH data");
            vSql.AppendLine("AS");
            vSql.AppendLine("(");
            vSql.AppendLine(valSelect);
            vSql.AppendLine(") ");
            vSql.AppendLine(" SELECT * ");
            vSql.AppendLine(" FROM data ");
            vSql.AppendLine(" WHERE row_id BETWEEN @Start AND @Finish ");
            vResult = vSql.ToString();
            return vResult;
        }
    }
}

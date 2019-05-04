using Seabury.Migrator.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seabury.Migrator.Contract
{
    public interface IDataInfoRepository
    {
        bool Load(DataInfo valInfo);
        bool ExportToXML(List<string> valListColumns, string valTableName, int valSizePerPage, string valDirectory);
        bool ExportReportByListToXML(List<string> valListReports, int valSizePerPage, string valDirectory);
    }
}

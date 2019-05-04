using Seabury.Migrator.Contract;
using Seabury.Migrator.SqlRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seabury.Migrator.AppService
{
    public class DataInfoAppService
    {
        private IDataInfoRepository _IDataInfoRepository;
        public bool ExportReportByListToXML(List<string> valListReports, int valSizePerPage, string valDirectory)
        {
            bool vResult = false;
            _IDataInfoRepository = new DataInfoRepository();
            _IDataInfoRepository.ExportReportByListToXML(valListReports, valSizePerPage, valDirectory);
            return vResult;
        }
    }
}

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


        public bool ExportToXML(Dictionary<string, string> valTableNameAndPK, int valSizePerPage, string valDirectory)
        {
            bool vResult = false;
            _IDataInfoRepository = new DataInfoRepository();
            _IDataInfoRepository.ExportToXML(valTableNameAndPK, valSizePerPage, valDirectory);
            return vResult;
        }

        public bool Import(string valDirectory)
        {
            bool vResult = false;
            _IDataInfoRepository = new DataInfoRepository();
            _IDataInfoRepository.ImportData( valDirectory);
            return vResult;
        }
    }

}    

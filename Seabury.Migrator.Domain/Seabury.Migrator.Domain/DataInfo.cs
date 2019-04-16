using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Seabury.Migrator.Domain
{
    [DataContract]
    
    public class DataInfo
    {
        [DataMember]
        public string Lote { get; set; }
        [DataMember]
        public DataTable Info { get; set; }

    }
}

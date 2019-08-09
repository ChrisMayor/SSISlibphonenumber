using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSISPhoneLibShape
{
    public class OutputColumn
    {
            public string Name { get; set; }

        public DataType DataType { get; set; }

        public string ColumnName { get; set; }

        public int LinageID { get; set; }
    }
}

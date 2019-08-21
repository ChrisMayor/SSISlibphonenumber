using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSISPhoneLibShape
{
    public class PhonenumberColumnInfo
    {
        public int PhoneNumberBufferIndex { get; set; }

        public int PhoneNumberIsoBufferIndex { get; set; }

        public string PhoneNumberIsoString { get; set; }

        public bool UsesIsoAsString { get; set; }


    }
}

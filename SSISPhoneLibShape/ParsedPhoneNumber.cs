using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSISPhoneLibShape
{
    public class ParsedPhoneNumber
    {

            public bool? IsViablePhoneNumber { get; set; }
            public string ExtractPossibleNumber { get; set; }

            public string NormalizedNumber { get; set; }

            public string NormalizedDigitsOnly { get; set; }

            public string E164Format { get; set; }

            public string IntFormat { get; set; }

            public bool IsValidNumber { get; set; }

            public int CountryCode { get; set; }

            public bool HasCountryCode { get; set; }

            public string PreferredDomesticCarrierCode { get; set; }

            public string GeoCoderDescription { get; set; }

    }
}

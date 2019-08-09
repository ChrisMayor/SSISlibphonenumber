using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSISPhoneLibShape
{
    public static class PhoneLibMethodConstants
    {
        public const string IsViablePhoneNumber = "IsViablePhoneNumber";
        public const string ExtractPossibleNumber = "ExtractPossibleNumber";

        public const string NormalizedNumber = "NormalizedNumber";

        public const string NormalizedDigitsOnly = "NormalizedDigitsOnly";
        public const string E164Format = "E164Format";
        public const string IntFormat = "IntFormat";
        public const string IsValidNumber = "IsValidNumber";
        public const string CountryCode = "CountryCode";
        public const string HasCountryCode = "HasCountryCode";

        public const string PreferredDomesticCarrierCode = "PreferredDomesticCarrierCode";
        public const string GeoCoderDescription = "GeoCoderDescription";


    }
}

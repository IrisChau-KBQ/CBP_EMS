using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CBP_EMS_SP.Common
{
    public class CBPRegularExpression
    {
        public static string contactNo = @"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{2})[-. )]*(\d{2})[-. ]*(\d{4})(?: *x(\d+))?\s*$";
        public static string IntergerValue = "^[0-9]+$";
        public static string Email = "^[_a-zA-Z0-9-]+(.[a-zA-Z0-9-].+)@[a-zA-Z0-9-]+(.[a-zA-Z0-9-]+)*(.[a-zA-Z]{2,4})$";
        //public static string Email = @"^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$";
        //public static string Email = @"^[\w\.=-]+@[\w\.-]+\.[\w]{2,}$";
        public static string StringExpression(int MinLength = 1, int MaxLength = 50, bool AllowNumbers = true, string AllowedSymbols = "", bool AllowAllSymbol = false)
        {
            if (AllowAllSymbol)
                return @"^.{" + MinLength + "," + MaxLength + "}$";
            else
                return @"^[a-zA-Z" + (AllowNumbers ? "0-9" : "") + AllowedSymbols + "]{" + MinLength + "," + MaxLength + "}$";

        }

        //public static string Url = @"(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?";
        public static string Url = @"^(http://)?(https://)?(www\.)?[A-Za-z0-9]+\.[a-z]{2,3}";
        public static bool RegExValidate(string Expression, string value)
        {

            Regex objRegEx = new Regex(Expression);
            return objRegEx.IsMatch(value);
        }
    }
}

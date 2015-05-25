using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneWPF.Models
{
    public class ModelBuildProperty
    {
        public string Name { get; set; }

        public Type UnitType { get; set; }

        public int Unit { get; set; }

        public string Conversion { get; set; }

        public NumberStyles NumberStyle { get; set; }

        public bool HasLeadingWhitespace
        {
            get { return Check(NumberStyles.AllowLeadingWhite); }
        }

        public bool HasTrailingWhitespace
        {
            get { return Check(NumberStyles.AllowTrailingWhite); }
        }

        public bool IsSigned
        {
            get { return Check(NumberStyles.AllowLeadingSign); }
        }

        public bool IsIntegral
        {
            get { return !Check(NumberStyles.AllowDecimalPoint); }
        }

        public bool IsHexadecimal
        {
            get { return Check(NumberStyles.AllowHexSpecifier); }
        }

        public bool IsSeparated
        {
            get { return Check(NumberStyles.AllowThousands); }
        }

        public bool IsGrouped
        {
            get { return Check(NumberStyles.AllowParentheses); }
        }

        public bool IsCurrency
        {
            get { return Check(NumberStyles.AllowCurrencySymbol); }
        }

        private bool Check(NumberStyles style)
        {
            return (NumberStyle & style) == style;
        }
    }
}

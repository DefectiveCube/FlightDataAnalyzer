using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace XPlaneWPF
{
    public class UnitTypeExtension : MarkupExtension
    {
        private Type _enumType;

        public UnitTypeExtension(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }

            _enumType = enumType;
        }

        public Type EnumType
        {
            get { return _enumType; }
            set
            {
                if (_enumType == value)
                {
                    return;
                }

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (!enumType.IsEnum)
                {
                    throw new ArgumentException("Type must be an Enum");
                }

                _enumType = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(EnumType);


            var result = from object enumValue in enumValues
                         select new EnumerationMember
                         {
                             Name = GetSupportedUnitName(enumValue),
                             Value = enumValue
                         };

            return result;
        }

        private string GetSupportedUnitName(object enumValue)
        {
            var name = EnumType
                .GetField(enumValue.ToString())
                .GetCustomAttribute<SupportedUnitType>();

            return name != null ? name.UnitTypeName : enumValue.ToString();
        }

        public class EnumerationMember
        {
            public string Name { get; set; }

            public object Value { get; set; }
        }
    }
}

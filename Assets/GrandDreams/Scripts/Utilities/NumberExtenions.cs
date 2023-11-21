using System;

namespace GrandDreams.Core.Utilities
{
    public static class NumberExtenions
    {
        public static float Round(this float number, int decimals)
        {
            if(decimals < 0)
            {
                decimals = 0;
            }
            var tmpDecimals = UnityEngine.Mathf.Pow(10, decimals);
            return UnityEngine.Mathf.Round(number * tmpDecimals) / tmpDecimals;
        }

        public static bool IsNumericType(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                case TypeCode.String:
                    double tmpNumber = 0;
                    return double.TryParse(o.ToString(), out tmpNumber);
                default:
                    return false;
            }
        }
    }

   
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace GrandDreams.Core.Utilities
{
    public static class StringExtensions
    {
        private static MD5 md5Hash = MD5.Create();

        public static T[] ToArray<T>(this string inputString, char splittingChar = ',')
        {
            string[] inputs = inputString.Split(splittingChar);

            T[] result = new T[inputs.Length];
            for (int index = 0; index < result.Length; index++)
            {
                result[index] = (T)Convert.ChangeType(inputs[index], typeof(T));
            }

            return result;
        }

        public static string Join<T>(this T[] array, string joiningString = ",")
        {
            return string.Join(joiningString, array.Select(x => x.ToString()).ToArray());
        }

        public static string Join<T>(this IEnumerable<T> array, string joiningString = ",")
        {
            return string.Join(joiningString, array.Select(x => x.ToString()).ToArray());
        }

        public static string Join<T>(this IEnumerable<T> array, char joiningChar)
        {
            return string.Join(joiningChar.ToString(), array.Select(x => x.ToString()).ToArray());
        }

        public static string Join<T>(this Dictionary<T, T> dictionary, string joiningString = ",")
        {
            return string.Join(joiningString, dictionary.Select(x =>  "Key: " + x.Key.ToString() + " | Value: " + x.Value.ToString()).ToArray());
        }

        public static T ParseToEnum<T>(this string enumString, T defaultValue = default(T)) where T : struct
        {
            if (string.IsNullOrEmpty(enumString))
            {
                return defaultValue;
            }

            T result = defaultValue;
            return Enum.TryParse<T>(enumString, true, out result) ? result : defaultValue;
        }

        public static string ToMD5(this string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static bool VerifyMD5(this string input, string hash)
        {
            string hashOfInput = input.ToMD5();

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string ToTimeString(this float time)
        {
            try
            {
                int iTime = (int)time;
                int minute = (iTime % 3600) / 60;
                int second = iTime % 60;
                double ms = Math.Round((time - iTime) * 1000);

                var result = string.Format("{0:00}:{1:00}:{2:000}", minute, second, ms);
                if (result.Length != 9)
                {
                    time += 0.001f;
                    return ToTimeString(time);
                }

                return string.Format("{0:00}:{1:00}:{2:000}", minute, second, ms);
            }
            catch (Exception)
            {
                return "00:00:000";
            }
        }

        public static string ToTimeString(this int time, bool isShowHour = false)
        {
            int hour = time / 3600;
            int minute = (time % 3600) / 60;
            int second = time % 60;

            if (isShowHour)
            {
                return string.Format("{0:00}:{1:00}:{2:00}", hour, minute, second);
            }
            else
            {
                return string.Format("{0:00}:{1:00}", minute, second);
            }
        }

        public static bool ToTimeNumber(this string timerString, out float timer)
        {
            timer = 0;

            string[] timerStringArray = timerString.Split(':');
            if (timerStringArray.Length != 3)
            {
                return false;
            }

            float minute, second, ticks;
            if (!float.TryParse(timerStringArray[0], out minute) || !float.TryParse(timerStringArray[1], out second) || !float.TryParse(timerStringArray[2], out ticks))
            {
                return false;
            }

            timer = minute * 60 + second + (ticks / 1000f).Round(3);

            return true;
        }

        public static void CopyToClipboard(this string str)
        {
            var textEditor = new UnityEngine.TextEditor();
            textEditor.text = str;
            textEditor.SelectAll();
            textEditor.Copy();
        }

        public static string RoundToString(this float number, int decimals = 3)
        {
            return number.Round(decimals).ToString();
        }

        public static bool IsValidEmail(this string email)
        {
            return Regex.IsMatch(email, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
        }

        public static string[] ToCharStringArray(this string stringValue)
        {
            return stringValue.ToCharArray().Select(x => x.ToString()).ToArray();
        }

        public static string ToCurrencyVND(this int number)
        {
            return string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:c}", number);
        }

        public static string ToCurrencyVND(this long number)
        {
            return string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:c}", number);
        }

        public static string ToCurrency(this int number, string format = "{0:#,0}{1}", string currency = "")
        {
            return string.Format(new System.Globalization.CultureInfo("vi-VN"), format, number, currency);
        }

        public static string ToCurrency(this long number, string format = "{0:#,0}{1}", string currency = "")
        {
            return string.Format(new System.Globalization.CultureInfo("vi-VN"), format, number, currency);
        }

        private static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public static string RemoveVietnameseSign(this string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }

        public static string FilterAlphaNumericCharacter(this string str)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            str = rgx.Replace(str, "");
            return str;
        }

        public static bool IsValidNormalString(this string str, string pattern = @"[A-Za-z0-9_\.]+")
        {
            return Regex.IsMatch(str, pattern);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDreams.Core.Components
{
    public class DateTimeInputField : InputField
    {
        #region Declare Variables


        public event System.Action<DateTimeInputField, System.DateTime> OnParsingDateTime_Succeed = delegate { };
        public event System.Action<DateTimeInputField> OnParsingDateTime_Failed = delegate { };

        private static readonly DateTime defaultDateTime = new DateTime(1, 1, 1);
        private static readonly char[] ALLOW_CHARACTER = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        private string formatParseToDateTime = "dd/MM/yyyy";
        public string FormatParseToDateTime
        {
            get
            {
                return formatParseToDateTime;
            }
            set
            {
                formatParseToDateTime = value;
            }
        }

        private DateTime dateTimeValue;
        public DateTime DateTimeValue
        {
            get
            {
                return dateTimeValue;
            }
            set
            {
                dateTimeValue = value;
                text = value.ToString(formatParseToDateTime);
            }
        }

        private int oldLength = 0;
        private char[] finalChars;
        private string tmpText;

        #endregion Declare Variables

        protected override void LateUpdate()
        {
            base.LateUpdate();
            if (m_Text.Length != oldLength)
            {
                if ((oldLength == 1 && m_Text.Length == 2) || (oldLength == 4 && m_Text.Length == 5))
                {
                    text += "/";
                    caretPosition = text.Length;
                }
                else if ((oldLength == 3 && m_Text.Length == 2) || (oldLength == 6 && m_Text.Length == 5))
                {
                    text = text.Substring(0, oldLength - 2);
                    caretPosition = text.Length;
                }

                if (text.Length != 0)
                {
                    finalChars = text.ToCharArray();
                    tmpText = "";
                    for (int index = 0; index < finalChars.Length; index++)
                    {
                        if (index == 2 || index == 5)
                        {
                            if (finalChars[index] == '/')
                            {
                                tmpText += finalChars[index];
                            }
                            else
                            {
                                text = "";
                                return;
                            }
                        }
                        else
                        {
                            if (ALLOW_CHARACTER.Contains(finalChars[index]))
                            {
                                tmpText += finalChars[index];
                            }
                            else
                            {
                                text = "";
                                return;
                            }
                        }
                    }

                    text = tmpText;

                    if (text.Length == 10)
                    {
                        try
                        {
                            int year = int.Parse(text.Substring(6, 4));
                            if (year > 2037 || year < 1902)
                            {
                                dateTimeValue = defaultDateTime;
                                if (OnParsingDateTime_Failed != null)
                                {
                                    OnParsingDateTime_Failed.Invoke(this);
                                }

                                text = "";
                            }
                        }
                        catch (Exception)
                        {
                            text = "";
                        }

                        if (!DateTime.TryParseExact(text, FormatParseToDateTime, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTimeValue))
                        {
                            dateTimeValue = defaultDateTime;
                            if (OnParsingDateTime_Failed != null)
                            {
                                OnParsingDateTime_Failed.Invoke(this);
                            }

                            text = "";

                            return;
                        }

                        if(OnParsingDateTime_Succeed != null)
                        {
                            OnParsingDateTime_Succeed(this, dateTimeValue);
                        }
#if UNITY_EDITOR
                        Debug.Log(dateTimeValue.ToString(formatParseToDateTime));
#endif
                    }

                    else if(text.Length > 10)
                    {
                        text = text.Substring(0, oldLength - 2);
                        caretPosition = text.Length;
                    }
                }

                oldLength = m_Text.Length;
            }
        }

        #region Public Function



        #endregion Public Function

        #region Private Function



        #endregion Private Function

        #region Event



        #endregion Event

        #region Editor



        #endregion Editor
    }
}
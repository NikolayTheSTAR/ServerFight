using UnityEngine;
using System;

namespace TheSTAR.Utility
{
    public static class TextUtility
    {
        public static string NumericValueToText(int value, NumericTextFormatType format) => NumericValueToText((float)value, format);
        public static string NumericValueToText(float value, NumericTextFormatType format)
        {
            switch (format)
            {
                case NumericTextFormatType.None: return value.ToString();

                case NumericTextFormatType.SimpleFloat:
                    int intPart = (int)value;
                    int floatPart = (int)((value - intPart) * 10);

                    if (floatPart == 0) return $"{intPart}";
                    else return $"{intPart}.{floatPart}";

                case NumericTextFormatType.RoundToInt: return ((int)value).ToString();

                case NumericTextFormatType.CompactFromK:

                    value = (int)value;

                    int bigPart;
                    int smallPart;

                    // до тысяч
                    if (value < 1000) return value.ToString();

                    // тысячи
                    else if (value < 1000000)
                    {
                        bigPart = (int)(value / 1000);

                        // Используем точку (5497 -> 5.4K)
                        if (bigPart < 10)
                        {
                            smallPart = (int)((value - (bigPart * 1000))/100);
                            return $"{bigPart}.{smallPart}K";
                        }

                        // Не используем точку (54978 -> 54K)
                        else return $"{bigPart}K";
                    }

                    // миллионы
                    else if (value < 1000000000)
                    {
                        bigPart = (int)(value / 1000000);

                        // Используем точку (5497097 -> 5.4M)
                        if (bigPart < 10)
                        {
                            smallPart = (int)((value - (bigPart * 1000000)) / 100000);
                            return $"{bigPart}.{smallPart}M";
                        }

                        // Не используем точку (54970971 -> 54M)
                        else return $"{bigPart}M";
                    }

                    // миллиарды
                    else
                    {
                        bigPart = (int)(value / 1000000000);

                        // Используем точку (5497097000 -> 5.4B)
                        if (bigPart < 10)
                        {
                            smallPart = (int)((value - (bigPart * 1000000000)) / 100000000);
                            return $"{bigPart}.{smallPart}B";
                        }

                        // Не используем точку (54970971000 -> 54B)
                        else return $"{bigPart}B";

                    }

                case NumericTextFormatType.XValue: return $"X{value}";
            }

            Debug.LogError("Не удалось выполнить преобразование");
            return "Error";
        }

        public static string TimeToText(TimeSpan value, bool useSeconds = true)
        {
            string result = "";

            if (value.Hours > 0) result += $"{value.Hours}h ";
            if (value.Minutes > 0) result += $"{value.Minutes}m ";
            if (useSeconds && value.Seconds > 0) result += $"{value.Seconds}s "; // todo написать типы форматов по аналогии с NumericTextFormatType

            return result;
        }

        public static string TimeToText(GameTimeSpan value, bool useSeconds = true)
        {
            string result = "";

            if (value.Hours > 0) result += $"{value.Hours}h ";
            if (value.Minutes > 0) result += $"{value.Minutes}m ";
            if (useSeconds && value.Seconds > 0) result += $"{value.Seconds}s "; // todo написать типы форматов по аналогии с NumericTextFormatType

            return result;
        }
        
        /*
        public static string TimeToText(GameDateTime value, TimeTextFormatType format)
        {
            string result = "";

            if (format == TimeTextFormatType.FullInTwoLines)
            {
                result = $"{value.month}.{value.day}.{value.year}\n{value.hour} hour";
            }
            else if (format == TimeTextFormatType.FullInOneLine)
            {
                result = $"y{value.year} m{value.month} d{value.day} h{value.hour}";
            }
            
            return result;
        }
        */
    }

    public enum NumericTextFormatType
    {
        /// <summary> Текст никак не форматируется и возвращается в исходном виде </summary>
        None,

        /// <summary> Значение округляется до одного символа после точки (Например 5.497 -> 5.4) </summary>
        SimpleFloat,

        /// <summary> Значение округляется до целых (Например 5.497 -> 5) </summary>
        RoundToInt,

        /// <summary> Значение представляется компактно от тысяч (Например 5497 -> 5.4K) </summary>
        CompactFromK,

        /// <summary>
        /// Например 58 -> X58
        /// </summary>
        XValue
    }

    public enum TimeTextFormatType
    {
        /// <summary>
        /// Полностью отображаем дату и время в две строки
        /// </summary>
        FullInTwoLines,
        FullInOneLine
    }
}
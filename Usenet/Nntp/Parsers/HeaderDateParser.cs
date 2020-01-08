using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Usenet.Nntp.Parsers
{
    internal static class HeaderDateParser
    {
        /// <summary>
        /// Parses header date/time strings as described in the
        /// <a href="https://tools.ietf.org/html/rfc5322#section-3.3">Date and Time Specification</a>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static DateTimeOffset? Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            string[] valueParts = value.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
            if (valueParts.Length > 2)
            {
                throw new FormatException(Resources.Nntp.BadHeaderDateFormat);
            }

            // skip day-of-week for now
            //string dayOfWeek = valueParts.Length == 2 ? valueParts[0] : null;

            string dateTime = valueParts.Length == 2 ? valueParts[1] : valueParts[0];

            // remove obsolete whitespace from time part
            dateTime = Regex.Replace(dateTime, @"\s+:\s+", ":");

            string[] dateTimeParts = dateTime.Split(new[] {' ', '\n', '\r', '\t'}, StringSplitOptions.RemoveEmptyEntries);
            if (dateTimeParts.Length != 5 && (dateTimeParts.Length != 6 || dateTimeParts[5] != "(UTC)"))
            {
                throw new FormatException(Resources.Nntp.BadHeaderDateFormat);
            }

            ParseDate(dateTimeParts, out int year, out int month, out int day);
            ParseTime(dateTimeParts[3], out int hour, out int minute, out int second);
            TimeSpan zone = ParseZone(dateTimeParts[4]);

            return new DateTimeOffset(year, month, day, hour, minute, second, 0, zone);
        }

        private static void ParseDate(string[] dateTimeParts, out int year, out int month, out int day)
        {
            if (dateTimeParts.Length < 3)
            {
                throw new FormatException(Resources.Nntp.BadHeaderDateFormat);
            }
            if (!int.TryParse(dateTimeParts[0], out day))
            {
                throw new FormatException(Resources.Nntp.BadHeaderDateFormat);
            }
            string monthString = dateTimeParts[1];
            int monthIndex = Array.FindIndex(DateTimeFormatInfo.InvariantInfo.AbbreviatedMonthNames,
                m => string.Equals(m, monthString, StringComparison.OrdinalIgnoreCase));
            if (monthIndex < 0)
            {
                throw new FormatException(Resources.Nntp.BadHeaderDateFormat);
            }
            month = monthIndex + 1;
            if (!int.TryParse(dateTimeParts[2], out year))
            {
                throw new FormatException(Resources.Nntp.BadHeaderDateFormat);
            }
            if (dateTimeParts[2].Length <= 2)
            {
                year += 100 * GetCentury(year, month, day);
            }
        }

        private static int GetCentury(int year, int month, int day)
        {
            DateTime today = DateTime.UtcNow.Date;
            int currentCentury = today.Year / 100;
            return new DateTime(currentCentury * 100 + year, month, day, 0, 0, 0, DateTimeKind.Utc) > today
                ? currentCentury - 1
                : currentCentury;
        }

        private static void ParseTime(string value, out int hour, out int minute, out int second)
        {
            string[] timeParts = value.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (timeParts.Length < 2 || timeParts.Length > 3)
            {
                throw new FormatException(Resources.Nntp.BadHeaderDateFormat);
            }
            if (!int.TryParse(timeParts[0], out hour))
            {
                throw new FormatException(Resources.Nntp.BadHeaderDateFormat);
            }
            if (!int.TryParse(timeParts[1], out minute))
            {
                throw new FormatException(Resources.Nntp.BadHeaderDateFormat);
            }
            second = 0;
            if (timeParts.Length > 2 && !int.TryParse(timeParts[2], out second))
            {
                throw new FormatException(Resources.Nntp.BadHeaderDateFormat);
            }
        }

        private static TimeSpan ParseZone(string value)
        {
            // The time zone must be as specified in RFC822, https://tools.ietf.org/html/rfc822#section-5

            if (!short.TryParse(value, out short zone))
            {
                switch (value)
                {
                    // UTC is not specified in RFC822, but allowing it since it is commonly used
                    case "UTC":
                    case "UT":
                    case "GMT":
                    case "Z":
                        break;

                    case "EDT":
                        zone = -0400;
                        break;

                    case "EST":
                    case "CDT":
                        zone = -0500;
                        break;

                    case "CST":
                    case "MDT":
                        zone = -0600;
                        break;

                    case "MST":
                    case "PDT":
                        zone = -0700;
                        break;

                    case "PST":
                        zone = -0800;
                        break;

                    case "A":
                        zone = -0100;
                        break;

                    case "N":
                        zone = +0100;
                        break;

                    case "M":
                        zone = -1200;
                        break;

                    case "Y":
                        zone = +1200;
                        break;

                    default:
                        throw new FormatException(Resources.Nntp.BadHeaderDateFormat);
                }
            }
            else if (-9999 > zone || zone > 9999)
            {
                throw new FormatException(Resources.Nntp.BadHeaderDateFormat);
            }

            int minute = zone % 100;
            int hour = zone / 100;
            return TimeSpan.FromMinutes(hour * 60 + minute);
        }
    }
}

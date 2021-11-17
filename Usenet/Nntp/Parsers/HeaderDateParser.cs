using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Usenet.Nntp.Parsers
{
    internal static class HeaderDateParser
    {
        private static Regex _dateTimeRegx = new Regex(@"(?:\s*(?<dayName>Sun|Mon|Tue|Wed|Thu|Fri|Sat),)?\s*(?<day>\d{1,2})\s+(?<month>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s+(?<year>\d{2,4})\s+(?<hour>\d{1,2}):(?<min>\d{1,2})(?::(?<sec>\d{1,2}))?\s*(?<tz>[+-]\d+|(?:UT|UTC|GMT|Z|EDT|EST|CDT|CST|MDT|MST|PDT|PST|A|N|M|Y|[A-Z]+))?", RegexOptions.IgnoreCase | RegexOptions.Compiled);

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

            var matches = _dateTimeRegx.Match(value);
            if (!matches.Success)
            {
                throw new FormatException(/*Resources.Nntp.BadHeaderDateFormat*/);
            }

            var day = int.Parse(matches.Groups["day"].Value);
            var month = matches.Groups["month"].Value;
            var year = int.Parse(matches.Groups["year"].Value);
            var hour = int.Parse(matches.Groups["hour"].Value);
            var minute = int.Parse(matches.Groups["min"].Value);
            int.TryParse(matches.Groups["sec"].Value, out var second);
            var tz = matches.Groups["tz"].Value;
            var zone = ParseZone(tz);

            int monthIndex = 1 + Array.FindIndex(DateTimeFormatInfo.InvariantInfo.AbbreviatedMonthNames,
                m => string.Equals(m, month, StringComparison.OrdinalIgnoreCase));

            if (matches.Groups["year"].Value.Length < 4)
            {
                year += GetCentury(year, monthIndex, day) * 100;
            }

            return new DateTimeOffset(year, monthIndex, day, hour, minute, second, 0, zone);
        }

        private static int GetCentury(int year, int month, int day)
        {
            DateTime today = DateTime.UtcNow.Date;
            int currentCentury = today.Year / 100;
            return new DateTime(currentCentury * 100 + year, month, day, 0, 0, 0, DateTimeKind.Utc) > today
                ? currentCentury - 1
                : currentCentury;
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
                    case "":
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

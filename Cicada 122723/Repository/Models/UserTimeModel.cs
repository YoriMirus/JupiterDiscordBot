using System;
using System.Collections.Generic;
using System.Text;

namespace Jupiter.Repository.Models
{
    //TODO, make TimeZone an enum or something else, string is too unsafe.

    /// <summary>
    /// Database model for storing time zone of a specific user.
    /// </summary>
    public class UserTimeModel : DatabaseObject
    {
        /// <summary>
        /// Name of this user visible by other users.
        /// </summary>
        public string Username { get; set; }

        public string Mention { get; set; }

        /// <summary>
        /// Timezone for this user as it is written in the database. Use the GetTimeSpan() method instead to get TimeSpan since UTC.
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Returns TimeSpan since the utc time. (UTC-1 would return a TimeSpan of -1 hour)
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTimeSpan()
        {
            string input = TimeZone.Trim().ToLower();
            bool isNegative = false;

            //return 0 hours, as it's exactly UTC time.
            if (input == "utc")
                return new TimeSpan(0);

            //Check if timezone is before or after utc
            if (input.StartsWith('-'))
            {
                isNegative = true;
                input = input.Substring(1);
            }
            else if (input.StartsWith('+'))
            {
                input = input.Substring(1);
            }

            string[] helper = input.Split(':');

            int hours;
            int minutes;

            if (helper.Length == 1)
            {
                hours = int.Parse(helper[0]);
                minutes = 0;
            }
            else
            {
                hours = int.Parse(helper[0]);
                minutes = int.Parse(helper[1]);
            }


            if (isNegative)
                return new TimeSpan(hours, minutes, 0).Negate();
            else
                return new TimeSpan(hours, minutes, 0);
        }

        public static bool ValidateTimeZoneString(string input)
        {
            string helper = input.ToLower().Trim();
            string[] helper2;

            if (helper.StartsWith("utc"))
                helper = helper.Substring(3);

            if(input.StartsWith('-') || input.StartsWith('+'))
            {
                helper = input.Substring(1);
            }

            helper2 = helper.Split(':');

            if (helper2.Length > 1)
            {
                //Check if both sides are parsable
                if (int.TryParse(helper2[0], out int left) && int.TryParse(helper2[0], out int right))
                {
                    if (left < 13 && right < 60)
                        return true;
                    return false;
                }
                else
                    return false;
            }
            else if (helper2.Length == 1)
            {
                if (int.TryParse(helper2[0], out int result))
                {
                    if (result < 13)
                        return true;
                }
            }
            return false;
        }
    }
}

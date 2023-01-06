using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum_FE.Util
{
    public class Utility
    {
        //password encryption
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        public static string SelecTime(DateTime createDate)
        {
            TimeSpan time = DateTime.Now - createDate;
            int days = time.Days;
            int seconds = time.Seconds;
            int minutes = time.Minutes;
            int hours = time.Hours;
            var selecTime = "No";
            if (days > 0 && days <= 7)
            {
                selecTime = days + " Day";
            }
            else if (hours > 0 && hours <= 23)
            {
                selecTime = days + " hour";
            }
            else if (minutes > 0 && minutes <= 59)
            {
                selecTime = minutes + " minute";
            }
            else if (seconds > 1 && seconds <= 59)
            {
                selecTime = seconds + " second";
            }
            return selecTime;
        }
    }
}

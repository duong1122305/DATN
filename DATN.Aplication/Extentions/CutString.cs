using DATN.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DATN.Aplication.Extentions
{
    public static class CutString
    {
        public static string CutName(string name)
        {
            string[] values = name.Split(' ');
            string result = "";
            var normalizedString = values[values.Length - 1].Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            result = stringBuilder.ToString().ToLower().Normalize(NormalizationForm.FormC);
            for (int i = 0; i < values.Length - 1; i++)
            {
                result += values[i][0].ToString().ToLower();
            }
            return result;
        }
        public static string RandomPass()
        {
            var result = "";
            Random random = new Random();
            result += Convert.ToChar(random.Next(65, 90));
            for (int i = 0; i < 6; i++)
            {
                if (random.Next(1, 100) % 2 == 0)
                {
                    result += Convert.ToChar(random.Next(97, 122));
                }
                else
                {
                    result += random.Next(0, 9);
                }
            }
            result += Convert.ToChar(random.Next(33, 43));
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteracyPro.Biz.Extensions
{
    public static class StringExtensions
    {

        public static string NullIfEmpty(this string val)
        {
            var x = TrimSafe(val);

            return string.IsNullOrWhiteSpace(x) ? null : x;
        }

        public static string TrimSafe(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return val;
            }

            return val.Trim();
        }

    }
}

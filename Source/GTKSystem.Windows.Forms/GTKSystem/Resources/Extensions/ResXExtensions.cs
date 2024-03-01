using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace GTKSystem.Resources.Extensions
{
    internal static class ResXExtensions
    {
        public static string OrThrowIfNullOrEmpty(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new System.Exception("string Is Null Or Empty");
            }
            else
                return value;
        }
        public static string OrThrowIfNull(this string value)
        {
            if (value == null)
            {
                throw new System.Exception("string Is Null Or Empty");
            }
            else
                return value;
        }
        public static ResXFileRef OrThrowIfNull(this ResXFileRef value)
        {
            if (value == null)
            {
                throw new System.Exception("string is null");
            }
            else
                return value;
        }

    }
}

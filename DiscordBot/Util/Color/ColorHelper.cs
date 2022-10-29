using System;
using System.Collections.Generic;
using System.Text;

namespace ArcadeBot.Util.Color
{
    public static class ColorHelper
    {
        public static uint GetColorFromHashCode(object src)
        {
            return Convert.ToUInt32(string.Concat("0x", src.GetHashCode().ToString().AsSpan(0, 6)), 16);
        }
    }
}

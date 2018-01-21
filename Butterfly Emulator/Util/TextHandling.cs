﻿using System;
using ButterStorm;

namespace Butterfly
{
    class TextHandling
    {
        internal static void Split(double k, out int a, out int b)
        {
            b = (int)Math.Round(k % 1 * 100);
            a = ((int)Math.Round(k * 100) - b) / 100;
        }

        internal static double Combine(int a, int b)
        {
            return a + ((float)b / 100);
        }

        internal static int Parse(string a)
        {
            int w = 0, i = 0, length = a.Length;

            if (length == 0)
                return 0;

            do
            {
                int k = a[i++];
                if (k < 48 || k > 59)
                    return 0;
                w = 10 * w + k - 48;
            }
            while (i < length);

             return w;
        }

        internal static string GetString(double k)
        {
            return k.ToString(OtanixEnvironment.cultureInfo);
        }

        internal static int BooleanToInt(bool k)
        {
            return k ? 1 : 0;
        }
    }
}

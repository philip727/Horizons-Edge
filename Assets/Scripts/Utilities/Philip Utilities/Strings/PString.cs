using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Philip.Utilities.String
{
    public static class PString
    {
        public static string FormatLong(this long num, string format)
        {
            if(format == "0:X")
            {
                if (num >= 1_000_000_000_000_000_000)
                {
                    return (num / 1_000_000_000_000_000_000D).ToString("0.#") + "Qn";
                }
                // 100Qr+
                else if (num >= 100_000_000_000_000_000)
                {
                    return (num / 1_000_000_000_000_000D).ToString("0.#") + "Q";
                }
                // 10Qr+
                else if (num >= 10_000_000_000_000_000)
                {
                    return (num / 1_000_000_000_000_000D).ToString("0.#") + "Q";
                }
                // 1Qr+
                else if (num >= 1_000_000_000_000_000)
                {
                    return (num / 1_000_000_000_000_000D).ToString("0.#") + "Q";
                }
                // 100T+
                else if (num >= 100_000_000_000_000)
                {
                    return (num / 1_000_000_000_000D).ToString("0.#") + "T";
                }
                // 10T+
                else if(num >= 10_000_000_000_000)
                {
                    return (num / 1_000_000_000_000D).ToString("0.#") + "T";
                }
                // 1T+
                else if(num >= 1_000_000_000_000)
                {
                    return (num / 1_000_000_000_000D).ToString("0.#") + "T";
                }
                // 100B+
                else if(num >= 100_000_000_000)
                {
                    return (num / 1_000_000_000D).ToString("0.#") + "B";
                }
                // 10B+
                else if (num >= 10_000_000_000)
                {
                    return (num / 1_000_000_000D).ToString("0.#") + "B";
                }
                // 1B+
                else if (num >= 1_000_000_000)
                {
                    return (num / 1_000_000_000D).ToString("0.#") + "B";
                }
                // 100M+
                else if (num >= 100_000_000)
                {
                    return (num / 1_000_000D).ToString("0.#") + "M";
                }
                // 10M+
                else if (num >= 10_000_000)
                {
                    return (num / 1_000_000D).ToString("0.#") + "M";
                }
                // 1M+
                else if (num >= 1_000_000)
                {
                    return (num / 1_000_000D).ToString("0.#") + "M";
                }
                // 100K+
                else if (num >= 100_000)
                {
                    return (num / 1000D).ToString("0.#") + "K";
                }
                // 10K+
                else if (num >= 10_000)
                {
                    return (num / 1000D).ToString("0.##") + "K";
                }
                else if (num >= 1_000)
                {
                    return (num / 1000D).ToString("0.##") + "K";
                }
            }

            return num.ToString("#,0");
        }
    }
}

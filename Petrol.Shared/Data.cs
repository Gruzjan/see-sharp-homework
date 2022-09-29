using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Petrol.Shared
{   
    public class Data
    {
        string lpgFile = Properties.Resources.lpg;
        public int pbRefillCount = 0;
        public int lpgRefillCount = 0;
        public decimal pbStatus = 45;
        public decimal lpgStatus = 30;
        public List<DateTime> lpgOnlyDays = new List<DateTime>();
        public DateTime firstLowOnGasDay;
        public string json = "[";
        public decimal lpgLiters = 0;
        public decimal pbLiters = 0;

        public Data()
        {
            string[] allLines = lpgFile.Split("\r\n");

            var distances = from line in allLines.Skip(1)
                            let elements = line.Split("\t")
                            select Convert.ToInt32(elements[1]);

            var dates = from line in allLines.Skip(1)
                        let elements = line.Split("\t")
                        select Convert.ToDateTime(elements[0]);

            for (int i = 0; i < distances.Count(); i++)
            {
                json += string.Format("{{\"Date\":\"{0}\",\"Przed\":{1},\"Po\":", dates.ElementAt(i).ToShortDateString(), lpgStatus.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));

                if (lpgStatus > 15)
                {
                    decimal result = Math.Round((9m * distances.ElementAt(i) / 100m), 2);
                    lpgLiters += result;
                    lpgStatus -= result;
                    lpgOnlyDays.Add(dates.ElementAt(i));
                }
                else
                {
                    if(lpgStatus < 5.25m && firstLowOnGasDay == DateTime.MinValue)
                        firstLowOnGasDay = dates.ElementAt(i);

                    decimal result = Math.Round((9m * distances.ElementAt(i) / 2 / 100m), 2);
                    lpgLiters += result;
                    lpgStatus -= result;

                    result = Math.Round((6m * distances.ElementAt(i) / 2 / 100m), 2);
                    pbLiters += result;
                    pbStatus -= result;
                }

                if (lpgStatus < 5)
                {
                    lpgRefillCount++;
                    lpgStatus = 30;
                }

                if(pbStatus < 40 && dates.ElementAt(i).DayOfWeek == DayOfWeek.Thursday)
                {
                    pbRefillCount++;
                    pbStatus = 45;
                }

                if (i == distances.Count() - 1)
                    json += string.Format("{0}}}]", lpgStatus.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
                else
                    json += string.Format("{0}}},", lpgStatus.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        public void saveToJson()
        {
            File.WriteAllText("./data.json", json);
        }

    }
}
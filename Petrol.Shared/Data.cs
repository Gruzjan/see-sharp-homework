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
        public int lpgOnlyDaysCount = 0;
        public DateTime firstLowOnGasDay;
        public List<DayData> daysData = new List<DayData>();
        public decimal lpgLitersUsed = 0;
        public decimal pbLitersUsed = 0;

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
                DayData day = new DayData();
                day.date = dates.ElementAt(i).ToShortDateString();
                day.before = lpgStatus;

                if (lpgStatus > 15)
                {
                    decimal result = Math.Round((9m * distances.ElementAt(i) / 100m), 2);
                    lpgLitersUsed += result;
                    lpgStatus -= result;
                    lpgOnlyDaysCount++;
                }
                else
                {
                    if(lpgStatus <= 5.25m && firstLowOnGasDay == DateTime.MinValue)
                        firstLowOnGasDay = dates.ElementAt(i);

                    decimal result = Math.Round((9m * distances.ElementAt(i) / 2 / 100m), 2);
                    lpgLitersUsed += result;
                    lpgStatus -= result;

                    result = Math.Round((6m * distances.ElementAt(i) / 2 / 100m), 2);
                    pbLitersUsed += result;
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

                day.after = lpgStatus;
                daysData.Add(day);
            }
        }

        public Tuple<decimal, decimal> getExpenses(decimal lpgPrice, decimal pbPrice, decimal gasTankPrice)
        {
            return Tuple.Create(Math.Round(lpgLitersUsed * lpgPrice, 2) + gasTankPrice, Math.Round(pbLitersUsed * pbPrice, 2));
        }
        

    }
}
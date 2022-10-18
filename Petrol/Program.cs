using Petrol.Shared;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;

public class Program
{
    public static void Main()
    {
        Data info = new Data();
        int op;
        bool cont = true;

        while (cont)
        {
            Console.WriteLine("[0] Exit\n[1] Refil counts\n[2] Days with lpg only\n[3] First day low on lpg\n[4] Log lpg tank status\n[5] Calculate cost");
            op = Convert.ToInt32(Console.ReadLine());

            switch (op)
            {
                case 0: cont = false; break;
                case 1: Console.Clear(); Console.WriteLine($"PB95: {info.pbRefillCount} times, LPG: {info.lpgRefillCount} times."); break;
                case 2: Console.Clear(); Console.WriteLine($"Lpg only days: {info.lpgOnlyDaysCount}"); break;
                case 3: Console.Clear(); Console.WriteLine($"First low on lpg day: {info.firstLowOnGasDay.ToShortDateString()}"); break;
                case 4: Console.Clear(); Console.WriteLine("Date\t\tBefore\tAfter");
                        foreach(var day in info.daysData)
                            Console.WriteLine($"{day.date}\t{day.before}\t{day.after}");
                        Console.WriteLine("Save data to JSON?\n[1] No, thanks [2] Yes, please");
                        op = Convert.ToInt32(Console.ReadLine());
                        switch (op)
                        {
                            case 1: break;
                            case 2: File.WriteAllText("./data.json", JsonSerializer.Serialize(info.daysData)); break;
                        }
                        break;
                case 5: Console.Clear();
                        decimal lpgLiterPrice = 2.29m, pbLiterPrice = 4.99m, gasTankPrice = 1600m;
                        Tuple<decimal, decimal> result = info.getExpenses(lpgLiterPrice, pbLiterPrice, gasTankPrice);
                        Console.WriteLine($"Koszt LPG: {result.Item1}\nKoszt PB95: {result.Item2}");
                        break;
            }
        }

    }
}
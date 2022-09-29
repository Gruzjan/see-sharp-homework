using Petrol.Shared;
using System.Linq.Expressions;

public class Program
{
    public static void Main()
    {
        Data info = new Data();

        // 1. zadanie
        //Console.WriteLine($"PB95: {info.pbRefillCount} razy, a LPG: {info.lpgRefillCount} razy.");


        // 2. zadanie
        /*foreach (var elem in info.lpgOnlyDays)
        {
            Console.WriteLine(elem);
        }*/


        // 3. zadanie
        //Console.WriteLine(info.firstLowOnGasDay);


        //4. zadanie
        //Console.WriteLine(info.json);
        //info.saveToJson();


        // 5. zadanie
        Console.WriteLine($"Koszt LPG: {Math.Round(info.lpgLiters * 2.29m, 2)} + 1600 = {Math.Round(info.lpgLiters * 2.29m + 1600, 2)}\nKoszt PB95: {Math.Round(info.pbLiters * 4.99m, 2)}");

    }
}

using ElectricConsumption;

var tariff = Tariff.CreateTariff(args);

var lines = await File.ReadAllLinesAsync(@"/Users/ash/Downloads/consumption.csv");

List<DailyConsumption> consumptionList = new List<DailyConsumption>();

Console.WriteLine($"[TARIFF]: PeakRate: {tariff.PeakRate}");

for (var index = 1; index < lines.Length; index++)
{
    var line = lines[index];
    var data = line.Split(",");

    if (data.Length == 3)
    {
        var readingStartDate = DateTimeOffset.Parse(data[1]).LocalDateTime;
        var consumption = GetValue<double>(data[0]);

        AddReadingToDay(readingStartDate, consumption, tariff);
    }
}

// foreach (var day in consumptionList.Select(x => new
//          {
//              x.Date, x.PeakConsumption, x.OffPeakConsumption, 
//              OffPeakCost = (x.OffPeakConsumption * tariff.OffPeakRate)/100,
//              PeakCost = (x.PeakConsumption * tariff.PeakRate)/100,
//              TotalCost = (x.OffPeakConsumption * tariff.OffPeakRate)/100 + (x.PeakConsumption * tariff.PeakRate)/100
//          }))
// {
//     Console.WriteLine($"[{day.Date:dd-MMM-yy}]: OffPeak {day.OffPeakConsumption:N4}\tPeak {day.PeakConsumption:n4}\tOffPeak Cost {day.OffPeakCost:C}\tPeakCost{day.PeakCost:C}\tTotalCost {day.TotalCost:C}");
// }

foreach (var day in consumptionList.Select(x => new
         {
             x.Date, x.PeakConsumption, x.OffPeakConsumption, 
             OffPeakCost = (x.OffPeakConsumption * tariff.OffPeakRate)/100,
             PeakCost = (x.PeakConsumption * tariff.PeakRate)/100,
             SolarOffSet = ((x.PeakConsumption - 283) * tariff.PeakRate)/100,
             TotalCost = (x.OffPeakConsumption * tariff.OffPeakRate)/100 + (x.PeakConsumption * tariff.PeakRate)/100
         })
             .GroupBy(x => x.Date.ToString("MMM yy"), (key, t)=> new
             {
                 Date = key,
                 OffPeakConsumption = t.Sum(y => y.OffPeakConsumption),
                 PeakConsumption = t.Sum(y => y.PeakConsumption),
                 OffPeakCost = t.Sum(y => y.OffPeakCost),
                 PeakCost = t.Sum(y => y.PeakCost),
                 TotalCost = t.Sum(y => y.TotalCost),
                 SolarSaving = 283 * tariff.PeakRate
             })
         )
{
    var defaultColor = Console.ForegroundColor;
    Console.Write($"[{day.Date.PadRight(10)}]:\tOffPeak {day.OffPeakConsumption:N4}\tPeak {day.PeakConsumption:n4}\tOffPeak Cost {day.OffPeakCost:C}\tPeakCost {day.PeakCost:C}\t\tTotalCost {day.TotalCost:C}");

    if (args.Contains("--showsolarsaving"))
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"\t(Solar Saving {day.SolarSaving/100:c})");
        Console.ForegroundColor = defaultColor;
    }
    Console.WriteLine();
}

void AddReadingToDay(DateTime dateTime, double consumption, Tariff tariff)
{
    if (consumptionList.All(x => x.Date != dateTime.Date))
    {
        consumptionList.Add(new DailyConsumption(dateTime.Date));
    }

    var dailyConsumption = consumptionList.Single(x => x.Date == dateTime.Date);

    if (dateTime.IsOffPeak(tariff.OffPeakStart, tariff.OffPeakEnd))
    {
        dailyConsumption.OffPeakConsumption += consumption;
    }
    else
    {
        dailyConsumption.PeakConsumption += consumption;
    }
}

T GetValue<T>(string value) 
{
    return (T)Convert.ChangeType(value, typeof(T));
}
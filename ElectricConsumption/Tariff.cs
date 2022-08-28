namespace ElectricConsumption;

public class Tariff
{
    public TimeSpan OffPeakStart { get; set; }

    public TimeSpan OffPeakEnd { get; set; }

    public double PeakRate { get; set; }

    public double OffPeakRate { get; set; }

    public static Tariff DefaultTariff => new Tariff
    {
        OffPeakRate = 5.24, PeakRate = 13.14,
        OffPeakEnd = new TimeSpan(0, 03, 30, 0),
        OffPeakStart = new TimeSpan(0, 22, 30, 0),
    };
    
    public  static Tariff  CreateTariff(string[] args)
    {
        var tariff = DefaultTariff;

        if (args.Length == 0) return tariff;
    
        var strings = args.ToList();
        tariff.PeakRate = tariff.PeakRate.GetValueFromArguments("--peakrate", strings);
        tariff.OffPeakRate = tariff.OffPeakRate.GetValueFromArguments("--offpeakrate", strings);
        tariff.OffPeakEnd = tariff.OffPeakEnd.GetValueFromArguments("--offpeanend", strings);
        tariff.OffPeakStart = tariff.OffPeakStart.GetValueFromArguments("--offpeanstart", strings);

        return tariff;
    }
}
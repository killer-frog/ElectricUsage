namespace ElectricConsumption;

public static class Extensions
{
    public static T GetValueFromArguments<T>(this T currentValue, string key, List<string> args)
    {
        if (args.Count == 0) return currentValue;

        var itmIdx = args.IndexOf(key);
        if (itmIdx >= -1)
        {
            try
            {
                return (T)Convert.ChangeType(args[itmIdx+1], typeof(T));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid value for key {key}");
            }
        }
        
        return currentValue;
    }

    public static string ToMonth(this int month) =>
        month switch
        {
            1 => "January",
            2 => "February",
            3 => "March",
            4 => "April",
            5 => "May",
            6 => "June",
            7 => "July",
            8 => "August",
            9 => "September",
            10 => "October",
            11 => "November",
            12 => "December",
            _ => string.Empty
        };

    public static bool IsOffPeak(this DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        return startTime.IsBeforeMidnight()
            ? !(date.TimeOfDay >= endTime && date.TimeOfDay < startTime)
            : date.TimeOfDay >= startTime && date.TimeOfDay < endTime;
    }

    public static bool IsBeforeMidnight(this TimeSpan time) => time.Hours is > 18 and <= 23;
    
    public static bool Between(this DateTime @this, DateTime minValue, DateTime maxValue) => minValue.CompareTo(@this) == -1 && @this.CompareTo(maxValue) == -1;
}
// See https://aka.ms/new-console-template for more information

var offPeakStartHour = 22;
var offPeakStartMin = 30;
int lengthHours = 5;

var startTime = new TimeSpan(0, offPeakStartHour, offPeakStartMin, 0);
var endTime = DateTime.Today.AddMinutes(startTime.TotalMinutes).AddHours(lengthHours).TimeOfDay;

var lines = await File.ReadAllLinesAsync(@"/Users/ashleyjackson/Downloads/consumption.csv");

for (var index = 1; index < lines.Length; index++)
{
    var line = lines[index];
    var data = line.Split(",");

    var readingStartDate = DateTimeOffset.Parse(data[1]).LocalDateTime;
    
    Console.WriteLine($"Date: ({data[1]}) {readingStartDate} IsOffPeak {readingStartDate.IsOffPeak(startTime, endTime)}");
}

public static class Extensions
{
    public static bool IsOffPeak(this DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        return startTime.IsBeforeMidnight()
            ? !(date.TimeOfDay >= endTime && date.TimeOfDay < startTime)
            : date.TimeOfDay >= startTime && date.TimeOfDay < endTime;
    }

    public static bool IsBeforeMidnight(this TimeSpan time) => time.Hours is > 18 and <= 23;
    
    public static bool Between(this DateTime @this, DateTime minValue, DateTime maxValue) => minValue.CompareTo(@this) == -1 && @this.CompareTo(maxValue) == -1;
}
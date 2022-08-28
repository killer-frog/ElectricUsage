namespace ElectricConsumption;

public  class DailyConsumption
{
    public DailyConsumption(DateTime date)
    {
        Date = date;
    }
    
    public DateTime  Date { get; set; }

    public double OffPeakConsumption { get; set; }

    public double PeakConsumption { get; set; }
}
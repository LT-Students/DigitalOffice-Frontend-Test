namespace LT.DigitalOffice.LoadTesting.Models.Common
{
  public record BaseFindFilter
  {
    public int SkipCount { get; set; }
    public int TakeCount { get; set; }

    public BaseFindFilter(
      int skipCount = 0,
      int takeCount = 1)
    {
      SkipCount = skipCount;
      TakeCount = takeCount;
    }
  }
}

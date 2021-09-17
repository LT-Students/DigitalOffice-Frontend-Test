namespace DigitalOffice.LoadTesting.Models.Time.Requests
{
    public record EditWorkTimeMonthLimitRequest
    {
        public float NormHours { get; set; }
        public string Holidays { get; set; }
    }
}

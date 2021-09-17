using System;

namespace DigitalOffice.LoadTesting.Helpers
{
    public static class CreatorUniqueName
    {
        public static string Generate()
        {
            return $"LoadTest{Guid.NewGuid()}{Guid.NewGuid()}";
        }
    }
}

using System.Collections.Generic;
using System.Text;

namespace DigitalOffice.LoadTesting.Helpers
{
    public static class CreatorJsonPatchDocument
    {
        public static string CreateJson(List<(string property, string newValue)> request)
        {
            StringBuilder builder = new();
            builder.Append('[');
            
            foreach(var pair in request)
            {
                builder.Append('{');

                builder.Append("\"path\":\"");
                builder.Append(pair.property);
                builder.Append("\",");

                builder.Append("\"op\":\"replace\",");

                builder.Append("\"value\":\"");
                builder.Append(pair.newValue);
                builder.Append('\"');

                builder.Append("},");
            }

            if (request.Count > 0) 
            {
                builder.Length = builder.Length - 1;
            }
            
            builder.Append(']');

            return builder.ToString();
        }
    }
}

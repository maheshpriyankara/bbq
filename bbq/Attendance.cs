using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace bbq
{
    public class Attendance : ApiController
    {
        public JObject Get(string empName, string year, string month)
        {
            JObject jsonObject = new JObject(
                new JProperty("name", "John Doe"),
                new JProperty("age", 42),
                new JProperty("address",
                    new JObject(
                        new JProperty("street", "123 Main St"),
                        new JProperty("city", "Anytown"),
                        new JProperty("state", "CA"),
                        new JProperty("zip", "12345")
                    )
                )
            );

            return jsonObject;
        }
    }
}
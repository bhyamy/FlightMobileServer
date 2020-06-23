using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightMobileApp.Models
{
    public class Command
    {
        [JsonPropertyName("Address")]
        public string Address { set; get; }

        [JsonPropertyName("Value"), Range(-1, 1)]
        public float Value { set; get; }
    }
}

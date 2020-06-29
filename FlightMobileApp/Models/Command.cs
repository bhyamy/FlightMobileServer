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
        [JsonPropertyName("aileron"), Range(-1, 1)]
        public float Aileron { get; set; }
        [JsonPropertyName("rudder"), Range(-1, 1)]
        public float Rudder { get; set; }
        [JsonPropertyName("elevator"), Range(-1, 1)]
        public float Elevator { get; set; }
        [JsonPropertyName("throttle"), Range(0, 1)]
        public float Throttle { get; set; }

    }
}

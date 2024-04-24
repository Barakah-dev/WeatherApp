using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WeatherApp.Models
{
    // [PrimaryKey("ID")]
    public class LocationWeatherEntity
    {
        // public string ID { get; set; }
        // [Required]

        // [Column (TypeName = "jsonb" )]
        // public string coord { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }
        public DateTime createdAt { get; set; }

        [Column (TypeName = "jsonb" )]
        public string weather { get; set; }
        
        public string @base { get; set; }

        [Column (TypeName = "jsonb" )]
        public string main { get; set; }
    
        public int visibility { get; set; }

        [Column (TypeName = "jsonb" )]
        public string wind { get; set; }
        
        [Column (TypeName = "jsonb" )]
        public string clouds { get; set; }
        public int dt { get; set; }

        [Column (TypeName = "jsonb" )]
        public string sys { get; set; }
        public int timezone { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }

    }
}
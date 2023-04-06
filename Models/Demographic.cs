using System;
using System.Collections.Generic;

#nullable disable

namespace Intex_app.Models
{
    public partial class Demographic
    {
        public string Id { get; set; }
        public string Sex { get; set; }
        public string SexGe { get; set; }
        public string HairColor { get; set; }
        public string AgeAtDeath { get; set; }
        public string AgeMethod { get; set; }
        public byte? Height { get; set; }
        public byte? AgeAtDeathMin { get; set; }
        public byte? AgeAtDeathMax { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedTimestamp { get; set; }
    }
}

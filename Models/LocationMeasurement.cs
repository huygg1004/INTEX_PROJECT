using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Intex_app.Models
{
    public partial class LocationMeasurement
    {
        
        public string Id { get; set; }
        public string Nors { get; set; }
        public string HighNs { get; set; }
        public string LowNs { get; set; }
        public string Eorw { get; set; }
        public string HighEw { get; set; }
        public string LowEw { get; set; }
        public string Square { get; set; }
        public string BurialNum { get; set; }
        public string Direction { get; set; }
        public string Depth { get; set; }
        public string HeadSouth { get; set; }
        public string FeetSouth { get; set; }
        public string HeadWest { get; set; }
        public string FeetWest { get; set; }
        public string BurialLength { get; set; }
        public string DiscoveryDay { get; set; }
        public string DiscoveryMonth { get; set; }
        public string DiscoveryYear { get; set; }
        public string Cluster { get; set; }
        public string BurialRack { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedTimestamp { get; set; }
    }
}

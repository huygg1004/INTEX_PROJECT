using System;
using System.Collections.Generic;

#nullable disable

namespace Intex_app.Models
{
    public partial class OsteologySkull
    {
        public string Id { get; set; }
        public byte? MaxCranialLength { get; set; }
        public byte? MaxCranialBreadth { get; set; }
        public byte? BasionBregmaHeight { get; set; }
        public byte? BasionNasion { get; set; }
        public byte? BasionProsthionLength { get; set; }
        public byte? NasionProsthion { get; set; }
        public byte? MaxNasalBreadth { get; set; }
        public string InterorbitalBreadth { get; set; }
        public string BizygomaticDiameter { get; set; }
        public string CranialSuture { get; set; }
        public string ZygomaticCrest { get; set; }
        public byte? NuchalCrest { get; set; }
        public byte? Gonian { get; set; }
        public byte? ParietalBossing { get; set; }
        public byte? OrbitEdge { get; set; }
        public byte? SupraorbitalRidges { get; set; }
        public byte? Robust { get; set; }
        public string SkullTrauma { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedTimestamp { get; set; }
    }
}

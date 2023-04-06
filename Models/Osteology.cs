using System;
using System.Collections.Generic;

#nullable disable

namespace Intex_app.Models
{
    public partial class Osteology
    {
        public string Id { get; set; }
        public string BasilarSuture { get; set; }
        public byte? VentralArc { get; set; }
        public byte? SubpubicAngle { get; set; }
        public byte? SciaticNotch { get; set; }
        public byte? PubicBone { get; set; }
        public byte? PreaurSulcus { get; set; }
        public byte? MedialIpramus { get; set; }
        public byte? DorsalPitting { get; set; }
        public string ForemanMagnum { get; set; }
        public byte? FemurHead { get; set; }
        public byte? HumerusHead { get; set; }
        public string Osteophytosis { get; set; }
        public string PubicSymphysis { get; set; }
        public byte? BoneLength { get; set; }
        public byte? MedialClavicle { get; set; }
        public byte? IliacCrest { get; set; }
        public byte? FemurDiameter { get; set; }
        public byte? Humerus { get; set; }
        public byte? FemurLength { get; set; }
        public byte? HumerusLength { get; set; }
        public byte? TibiaLength { get; set; }
        public string PostcraniaTrauma { get; set; }
        public string ToothAttrition { get; set; }
        public string ToothEruption { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedTimestamp { get; set; }
    }
}

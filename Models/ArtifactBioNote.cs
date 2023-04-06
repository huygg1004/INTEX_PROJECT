using System;
using System.Collections.Generic;

#nullable disable

namespace Intex_app.Models
{
    public partial class ArtifactBioNote
    {
        public string Id { get; set; }
        public string Rack { get; set; }
        public string ArtifactFound { get; set; }
        public string ArtifactDescription { get; set; }
        public string SampleTaken { get; set; }
        public string BioNotes { get; set; }
        public string AdditionalNotes { get; set; }
        public string FaceBundle { get; set; }
        public string PathologyAnomalies { get; set; }
        public string BurialWraping { get; set; }
        public string PreservationIndex { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedTimestamp { get; set; }
    }
}

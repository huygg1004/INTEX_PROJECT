using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Models.ViewModels
{
    public class IndexViewModel
    {
        public string pageid { get; set; }

        public List<LocationMeasurement> LocationMeasurements { get; set; }
        public List<Osteology> Osteologies { get; set; }
        public List<OsteologySkull> OsteologySkulls { get; set; }
        public List<Demographic> Demographics { get; set; }
        public List<ArtifactBioNote> ArtifactBioNotes { get; set; }

        public List<LocationMeasurement> LocationMeasurementsR { get; set; }
        public List<Osteology> OsteologiesR { get; set; }
        public List<OsteologySkull> OsteologySkullsR { get; set; }
        public List<Demographic> DemographicsR { get; set; }
        public List<ArtifactBioNote> ArtifactBioNotesR { get; set; }

        public PageNumberingInfo PageNumberingInfo { get; set; }

       
    }
}

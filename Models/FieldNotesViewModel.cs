using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Models
{
    public class FieldNotesViewModel
    {
        public Osteology Osteology { get; set; }
        public OsteologySkull OsteologySkull { get; set; }
        public LocationMeasurement LocationMeasurement { get; set; }
        public Demographic Demographic { get; set; }
        public ArtifactBioNote ArtifactBioNote { get; set; }
        public String Identifier { get; set; }
    }
}

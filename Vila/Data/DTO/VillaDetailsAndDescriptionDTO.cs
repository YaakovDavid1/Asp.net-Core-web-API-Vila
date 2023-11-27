using System.Collections.Generic;
using Vila.Data.Entities;

namespace Vila.Data.DTO
{
    public class VillaDetailsAndDescriptionDTO
    {
        public VillaDetailsEO VillaDetails { get; set; }
        public VillaDescriptionEO VillaDescription { get; set; }
        public CheckListVilaEO CheckListVila { get; set; }

    }
}

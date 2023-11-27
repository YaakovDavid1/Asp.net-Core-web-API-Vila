using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vila.Data.Entities
{

    [Table("VilaPhotos")]
    public class VilaPhoto
    {
        [Key]
        public int PhotoID { get; set; }
        public int VilaId { get; set; }
        public byte[] FilePath { get; set; }

        public virtual VillaDetailsEO VillaDetails { get; set; }


    }

}

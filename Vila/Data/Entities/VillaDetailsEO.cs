using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vila.Data.Entities
{
    [Table("VillaDetails")]
    public class VillaDetailsEO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int VilaId { get; set; }
        public int UserID { get; set; }
        public string NameVila { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int NumBedroom { get; set; }
        public int NumBathroom { get; set; }
        public int NumShower { get; set; }

        public virtual UsersEO OwnerUser { get; set; }
        public virtual VillaDescriptionEO vilaDescription { get; set; }
        public virtual CheckListVilaEO CheckListVila { get; set; }
        public virtual ICollection<VilaPhoto> Photos { get; set; }
    }
}

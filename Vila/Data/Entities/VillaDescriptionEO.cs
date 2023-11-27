using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vila.Data.Entities
{
    [Table("VillaDescription")]
    public class VillaDescriptionEO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VilaId { get; set; }
        public string AboutComplex { get; set; }
        public string OutDescription { get; set; }
        public string InDescription { get; set; }
        public string Audience { get; set; }
        public string ImportantInfo { get; set; }

        public virtual VillaDetailsEO villaDetails { get; set; }
    }
}

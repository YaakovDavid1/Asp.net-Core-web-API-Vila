using System.ComponentModel.DataAnnotations;

namespace Vila.Data.DTO
{
    public class VillaDetailsDTO
    {
        [Key]
        public int VilaId { get; set; }
        public int UserID { get; set; }
        public string NameVila { get; set; }
    }
}

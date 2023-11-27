using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vila.Data.Entities
{
    [Table("Test")]
    public class Test
    {
        [Key]
        public string name { get; set; }
        public string city { get; set; }
    }
}

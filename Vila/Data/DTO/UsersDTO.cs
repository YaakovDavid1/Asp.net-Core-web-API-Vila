using System.ComponentModel.DataAnnotations;

namespace Vila.Data.DTO
{
    public class UsersDTO
    {
        [Key]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
    }
}

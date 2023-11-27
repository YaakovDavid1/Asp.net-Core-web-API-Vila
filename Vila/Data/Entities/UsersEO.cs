using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vila.Data.Entities
{
    [Table("Users")]
    public class UsersEO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }

        //  public string ResetPasswordToken { get; set; }
        //   public DateTime ResetPasswordExpiry { get; set; }

        public virtual ICollection<VillaDetailsEO> VillaDetails { get; set; }




    }
}

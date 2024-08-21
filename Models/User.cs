using System.ComponentModel.DataAnnotations;

namespace QR_Menu.Models{
    public class User{
        [Key]
        public int UserId{set;get;}

        public string ?Name{set;get;}
        [EmailAddress]
        public string ?UserEmail{set;get;}
        public string ?Password{set;get;}

    }
}
using System.ComponentModel.DataAnnotations;

namespace QR_Menu.Models{
    public class Menu{
        [Key]
        public int MenuId{set;get;}
        public string ?MenuName{set;get;}
        public int OwnerId{set;get;}
        
        public string ?QR_Image{set;get;}


    }
}
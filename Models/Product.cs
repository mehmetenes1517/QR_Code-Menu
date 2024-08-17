using System.ComponentModel.DataAnnotations;

namespace QR_Menu.Models{
    public class Product{

        [Key]
        public int ProductId{set;get;}
        public string ?ProductName{set;get;}
        public int ProductPrice{set;get;}
        public string ?Category{set;get;}
        public int MenuId{set;get;}

    }
}
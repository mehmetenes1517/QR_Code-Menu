using Microsoft.EntityFrameworkCore;
using QR_Menu.Models;

namespace QR_Menu.Data{
    public class DataContext:DbContext{
        public DbSet<User> Users{set;get;}
        public DbSet<Menu> Menus{set;get;}
        public DbSet<Product> Products{set;get;}

        public DataContext(DbContextOptions<DataContext> options):base(options){
            
        }
    }
}
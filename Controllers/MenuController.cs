using Microsoft.AspNetCore.Mvc;
using QR_Menu.Data;
using QR_Menu.Models;

namespace QR_Menu.Controllers{
    public class MenuController:Controller{
        DataContext _context{set;get;}
        public MenuController(DataContext context){
            _context=context;
        }   
        [HttpGet("Menu/{id}")]
        public IActionResult Index(int ?id){
          
                if(id==null){
                    return NotFound();
                }
                var menu=_context.Menus.FirstOrDefault(p=>p.MenuId==id);
       
                    var products=_context.Products.Where(p=>p.MenuId==menu!.MenuId).ToList<Product>();
                    
                    return View(products);
                
        }



    }
}
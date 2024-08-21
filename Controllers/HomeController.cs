using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QR_Menu.Models;
using QR_Menu.Controllers;
using QR_Menu.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace QR_Menu.Controllers{
    public class HomeController : Controller
    {
        private DataContext _context{set;get;}
        public HomeController(DataContext context){
            this._context=context;
        }

        
        public IActionResult Index()
        {
            if(User?.Identity?.Name!=null){
                ViewData["Name"]=_context?.Users?.FirstOrDefault(p=>p.UserEmail==User.Identity.Name)?.Name;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User user){

            var user1=_context.Users.FirstOrDefault(p=>p.UserEmail==user.UserEmail && p.Password==user.Password);
            if(user1!=null){


                //Cookie Authentication--------------------------------------------------------------------

                var claims=new List<Claim>(){
                    new Claim(ClaimTypes.Name,user1.UserEmail??""),
                    new Claim(ClaimTypes.Role,"User")
                };
                var claimsIdentity=new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                var properties=new AuthenticationProperties(){};

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                new ClaimsPrincipal(claimsIdentity),
                                                properties);
                //------------------------------------------------------------------------------------------------
                return RedirectToAction("Index","Home");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Login(){
            
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Signup(User user){
            if(_context.Users.Where(p=>p.UserEmail==user.UserEmail).Count()==0)
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login","Home");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Signup(){
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Signout(){
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public IActionResult ViewMenu(){
            if(User.Identity?.IsAuthenticated==true){
                var user1=_context.Users.FirstOrDefault(p=>p.UserEmail==User.Identity.Name);
                
                var list=_context.Menus.Where(p=>p.OwnerId==user1.UserId).ToList<Menu>();

                return View(list);

            }
            else{
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult ViewMenu(int id){
            if(User.Identity?.IsAuthenticated==true){
                
                //View
                if(id>0){

                }
                //Delete
                else{

                    var menu=_context.Menus.FirstOrDefault(p=>p.MenuId==(-id));
                    var product_list=_context.Products.Where(p=>p.MenuId==menu.MenuId).ToList<Product>();

                    foreach (var i in product_list){
                        _context.Products.Remove(i);
                    }
                    _context.Menus.Remove(menu!);
                    _context.SaveChanges();
                    return RedirectToAction("ViewMenu","Home");

                }



                
                var user1=_context.Users.FirstOrDefault(p=>p.UserEmail==User.Identity.Name);
                
                var list=_context.Menus.Where(p=>p.OwnerId==user1.UserId).ToList<Menu>();

                return View(list);

            }
            else{
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult CreateMenu(Menu menu){
            if(User.Identity?.IsAuthenticated==true){
                var user1=_context.Users.FirstOrDefault(p=>p.UserEmail==User.Identity.Name);
                menu.OwnerId=user1.UserId;



                _context.Menus.Add(menu);
                _context.SaveChanges();





                return RedirectToAction("Index","Home");

            }
            else{
                return NotFound();
            }
        }
        [HttpGet]
        public IActionResult CreateMenu(){
            if(User.Identity?.IsAuthenticated==true){
                return View();

            }
            else{
                return NotFound();
            }
        }
        [HttpGet]
        public IActionResult EditMenu(){
            if(User.Identity?.IsAuthenticated==true){
                var user=_context.Users.FirstOrDefault(p=>p.UserEmail==User.Identity.Name);
                var menus=_context.Menus.Where(p=>p.OwnerId==user.UserId).ToList<Menu>();
                return View(menus);


            }
            else{
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditMenu(string ProductName,int ProductPrice,string ProductCategory,int MenuId){
            if(User.Identity?.IsAuthenticated==true){
                Product product1=new Product{
                     ProductName=ProductName,
                     ProductPrice=ProductPrice,
                     Category=ProductCategory,
                     MenuId=MenuId
                };
                await _context.Products.AddAsync(product1);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index","Home");
            }else{
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult MenuList(int ?id){
          
                if(id==null){
                    return NotFound();
                }
                var menu=_context.Menus.FirstOrDefault(p=>p.MenuId==id);
       
                    var products=_context.Products.Where(p=>p.MenuId==menu!.MenuId).ToList<Product>();
                    
                    return View(products);
                
        }
       
    }

}
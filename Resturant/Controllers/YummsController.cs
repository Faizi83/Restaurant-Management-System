using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resturant.Db;
using Resturant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


namespace Resturant.Controllers
{
    public class YummsController : Controller
    {

        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        public YummsController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
        }


        
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {

            var foods = dbContext.allfoods.Take(9).ToList();

            //For passing cart counts
            var cartItems = dbContext.cart.ToList();
            ViewBag.CartItemCount = cartItems.Count; 

            return View(foods);
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login");
        }

        [Route("login")]
        [HttpGet]
        public IActionResult login()
        {
            return View();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await dbContext.Registers.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                // Create claims and identity
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                if (user.Role == "Admin")
                {
                    return RedirectToAction("activeitems"); // Admin dashboard
                }
                else
                {
                    return RedirectToAction("Index"); // User dashboard
                }
            }

            ViewBag.ErrorMessage = "Incorrect Email or Password.";
            return View();
        }

        [Route("register")]
        [HttpGet]
        public IActionResult register()
        {
            return View();
        }
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> register(register model)
        {
          
                var reg = new register
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    Role = "User" // Default role is User
                };

                await dbContext.Registers.AddAsync(reg);
                await dbContext.SaveChangesAsync();

            return RedirectToAction("login");
            }

        [Route("unauthorized")]
        public IActionResult unauthorized()
        {
            return View();
        }




        [Route("about")]
        public IActionResult about()
        {

            //For passing cart counts
            var cartItems = dbContext.cart.ToList();
            ViewBag.CartItemCount = cartItems.Count;

            return View();
        }




        [Route("booking")]
        [HttpGet]
     

        public IActionResult booking()
        {
            //For passing cart counts
            var cartItems = dbContext.cart.ToList();
            ViewBag.CartItemCount = cartItems.Count;

            return View();
        }


        [Route("booking")]
        [HttpPost]
        public async Task<IActionResult> booking(bookedtables viewmodel)
        {
          

       

                var book = new bookedtables
                {
                    Name = viewmodel.Name,
                    PhoneNumber = viewmodel.PhoneNumber,
                    Email = viewmodel.Email,
                    Persons = viewmodel.Persons,
                    Date = viewmodel.Date
                };

                await dbContext.AddAsync(book);
                await dbContext.SaveChangesAsync();


                return View();
            }


        [Route("cart")]
        [HttpGet]
        public IActionResult cart()
        {
            var cartItems = dbContext.cart.ToList();
            ViewBag.CartItemCount = cartItems.Count; // Pass the count of cart items to the view

            return View(cartItems);
        }





        [Route("menu")]
        [HttpGet]
        public IActionResult Menu(string searchTerm, string category)
        {
            var foods = dbContext.allfoods.ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                foods = foods.Where(f => f.foodname.Contains(searchTerm)).ToList();
            }

            if (!string.IsNullOrEmpty(category) && category != "All Types")
            {
                foods = foods.Where(f => f.foodcategory == category).ToList();
            }

            var viewModel = new MenuViewModel
            {
                Foods = foods,
                SearchTerm = searchTerm,
                Category = category,
                IsFoodAvailable = foods.Any()
            };
                
            //For passing cart counts
            var cartItems = dbContext.cart.ToList();
            ViewBag.CartItemCount = cartItems.Count;

            return View(viewModel);
        }

        [Route("food/GetCartItemCount")]
        [HttpGet]
        public JsonResult GetCartItemCount()
        {
            var cartItems = dbContext.cart.ToList();
            return Json(cartItems.Count);
        }

        [Route("food")]
        [HttpGet]
        public IActionResult foodpage(int Id)
        {
            var item = dbContext.allfoods.FirstOrDefault(f => f.Id == Id);

            //For passing cart counts
            var cartItems = dbContext.cart.ToList();
            ViewBag.CartItemCount = cartItems.Count;

            return View(item);
        }

        [Route("food")]
        [HttpPost]
        public async Task<IActionResult> foodpage([FromBody] addtocart viewmodel)
        {
            if (viewmodel == null)
            {
                return BadRequest("Invalid data.");
            }

            // Check if the product already exists in the cart
            var existingCartItem = await dbContext.cart
                .FirstOrDefaultAsync(ci => ci.CartId == viewmodel.CartId);

            if (existingCartItem != null)
            {
                return BadRequest("Product already in cart.");
            }

            var cartitem = new addtocart
            {
                CartId = viewmodel.CartId,
                Name = viewmodel.Name,
                Image = viewmodel.Image,
                Qty = viewmodel.Qty,
                Price = viewmodel.Price,
            };

            await dbContext.AddAsync(cartitem);
            await dbContext.SaveChangesAsync();

           



            return Ok(new { message = "Added to cart successfully" });
        }



        [Route("booktable")]
        [HttpGet]

        public IActionResult booktable()
        {
            var book = dbContext.bookings.ToList();
            return View(book);
        }

        [Route("booktable/delete/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBooking(int id)
        {
            var booking = dbContext.bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                dbContext.bookings.Remove(booking);
                dbContext.SaveChanges();
                return RedirectToAction("booktable");
            }
            return NotFound();
        }




        [Route("additems")]
        [HttpGet]
        public IActionResult additems()
        {
            return View();
        }

        [Route("additems")]
        [HttpPost]
        public async Task<IActionResult> additems(addfood viewmodel, IFormFile foodimage)
        {
            string imageurl = null;

            if (foodimage != null)
            {
                string uploadsfolder = Path.Combine(webHostEnvironment.WebRootPath, "foodimages");
                Directory.CreateDirectory(uploadsfolder);

                string uniquefilename = Guid.NewGuid().ToString() + "_" + foodimage.FileName;
                string filepath = Path.Combine(uploadsfolder, uniquefilename);

                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    await foodimage.CopyToAsync(filestream);
                    imageurl = "/foodimages/" + uniquefilename;
                }

                var food = new addfood
                {
                    foodname = viewmodel.foodname,
                    foodprice = viewmodel.foodprice,
                    foodcategory = viewmodel.foodcategory,
                    fooddesc = viewmodel.fooddesc,
                    foodimage = imageurl
                };

                await dbContext.AddAsync(food);
                await dbContext.SaveChangesAsync();

                return View(); // Assuming you have a view to return to after successful addition
            }

         
            return BadRequest("Food image is required.");
        }






   


        [Route("updateitems")]
        [HttpGet]
        public async Task<IActionResult> updateitems(int id)
        {
            var food = await dbContext.allfoods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }



        [Route("updateitems")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, addfood viewModel, IFormFile foodimage)
        {


            var food = await dbContext.allfoods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            if (foodimage != null)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(food.foodimage))
                {
                    string oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, food.foodimage.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save new image
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "foodimages");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + foodimage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await foodimage.CopyToAsync(fileStream);
                }

                food.foodimage = "/foodimages/" + uniqueFileName;
            }

            food.foodname = viewModel.foodname;
            food.foodprice = viewModel.foodprice;
            food.foodcategory = viewModel.foodcategory; 

            dbContext.allfoods.Update(food);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(activeitems));
        }


       
        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            var food = await dbContext.allfoods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(food.foodimage))
            {
                string oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, food.foodimage.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            dbContext.allfoods.Remove(food);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(activeitems));
        }



        [HttpPost]

        public async Task<IActionResult> Del(int id)
        {
            var cartitem = await dbContext.cart.FindAsync(id);
            if (cartitem == null)
            {
                return NotFound();
            }



            dbContext.cart.Remove(cartitem);    
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(cart));
        }








        [Route("activeitems")]
            [HttpGet]
            public IActionResult activeitems()
            {

            var actives = dbContext.allfoods.ToList();
                return View(actives);
            }
        }
    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace SocialNetwork.Controllers
{
    public class UsersController : Controller
    {
        private readonly SocialNetworkContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;


        public UsersController(SocialNetworkContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var socialNetworkContext = _context.User.Include(u => u.City).Include(u => u.Country).Include(u => u.SecurityQuestion);
            return View(await socialNetworkContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.City)
                .Include(u => u.Country)
                .Include(u => u.SecurityQuestion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.CurrentProfileImage = user.ProfilePicture;
            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["CityID"] = new SelectList(_context.City, "Id", "CityName");
            ViewData["CountryID"] = new SelectList(_context.Country, "Id", "CountryName");
            ViewData["SecurityQuestionID"] = new SelectList(_context.SecurityQuestion, "Id", "Question");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Phone,Gender,Age,Email,Password,UserType,DateOfBirth,ProfileImage,SecurityQuestionAnswer,SecurityQuestionID,CityID,CountryID")] User user)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(user);
                user.ProfilePicture = uniqueFileName;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityID"] = new SelectList(_context.City, "Id", "CityName", user.CityID);
            ViewData["CountryID"] = new SelectList(_context.Country, "Id", "CountryName", user.CountryID);
            ViewData["SecurityQuestionID"] = new SelectList(_context.SecurityQuestion, "Id", "Question", user.SecurityQuestionID);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["CityID"] = new SelectList(_context.City, "Id", "CityName", user.CityID);
            ViewData["CountryID"] = new SelectList(_context.Country, "Id", "CountryName", user.CountryID);
            ViewData["SecurityQuestionID"] = new SelectList(_context.SecurityQuestion, "Id", "Question", user.SecurityQuestionID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Phone,Gender,Age,Email,Password,UserType,DateOfBirth,ProfileImage,ProfilePicture,SecurityQuestionAnswer,SecurityQuestionID,CityID,CountryID")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityID"] = new SelectList(_context.City, "Id", "CityName", user.CityID);
            ViewData["CountryID"] = new SelectList(_context.Country, "Id", "CountryName", user.CountryID);
            ViewData["SecurityQuestionID"] = new SelectList(_context.SecurityQuestion, "Id", "Question", user.SecurityQuestionID);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.City)
                .Include(u => u.Country)
                .Include(u => u.SecurityQuestion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        private string UploadedFile(User model)
        {
            string uniqueFileName = null;

            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                if (!String.IsNullOrEmpty(model.ProfilePicture))
                {
                    string previousFilePath = Path.Combine(uploadsFolder, model.ProfilePicture);
                    try
                    {
                        System.IO.File.Delete(previousFilePath);
                    }
                    catch (IOException copyError)
                    {
                        Console.WriteLine(copyError.Message);
                    }
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}

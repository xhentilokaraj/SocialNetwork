using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class UserRelationshipsController : Controller
    {
        private readonly SocialNetworkContext _context;

        public UserRelationshipsController(SocialNetworkContext context)
        {
            _context = context;
        }

        // GET: UserRelationships
        public async Task<IActionResult> Index()
        {
            var socialNetworkContext = _context.UserRelationship.Include(u => u.RelatedUser).Include(u => u.RelatingUser);
            return View(await socialNetworkContext.ToListAsync());
        }

        // GET: UserRelationships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRelationship = await _context.UserRelationship
                .Include(u => u.RelatedUser)
                .Include(u => u.RelatingUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRelationship == null)
            {
                return NotFound();
            }

            return View(userRelationship);
        }

        // GET: UserRelationships/Create
        public IActionResult Create()
        {
            ViewData["RelatedUserId"] = new SelectList(_context.User, "Id", "Email");
            ViewData["RelatingUserId"] = new SelectList(_context.User, "Id", "Email");
            return View();
        }

        // POST: UserRelationships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RelatingUserId,RelatedUserId,RequestStatus,RelationshipStatus,DateOfRequest,DateOfAcceptance")] UserRelationship userRelationship)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userRelationship);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RelatedUserId"] = new SelectList(_context.User, "Id", "Email", userRelationship.RelatedUserId);
            ViewData["RelatingUserId"] = new SelectList(_context.User, "Id", "Email", userRelationship.RelatingUserId);
            return View(userRelationship);
        }

        // GET: UserRelationships/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRelationship = await _context.UserRelationship.FindAsync(id);
            if (userRelationship == null)
            {
                return NotFound();
            }
            ViewData["RelatedUserId"] = new SelectList(_context.User, "Id", "Email", userRelationship.RelatedUserId);
            ViewData["RelatingUserId"] = new SelectList(_context.User, "Id", "Email", userRelationship.RelatingUserId);
            return View(userRelationship);
        }

        // POST: UserRelationships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RelatingUserId,RelatedUserId,RequestStatus,RelationshipStatus,DateOfRequest,DateOfAcceptance")] UserRelationship userRelationship)
        {
            if (id != userRelationship.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRelationship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRelationshipExists(userRelationship.Id))
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
            ViewData["RelatedUserId"] = new SelectList(_context.User, "Id", "Email", userRelationship.RelatedUserId);
            ViewData["RelatingUserId"] = new SelectList(_context.User, "Id", "Email", userRelationship.RelatingUserId);
            return View(userRelationship);
        }

        // GET: UserRelationships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRelationship = await _context.UserRelationship
                .Include(u => u.RelatedUser)
                .Include(u => u.RelatingUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRelationship == null)
            {
                return NotFound();
            }

            return View(userRelationship);
        }

        // POST: UserRelationships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userRelationship = await _context.UserRelationship.FindAsync(id);
            _context.UserRelationship.Remove(userRelationship);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRelationshipExists(int id)
        {
            return _context.UserRelationship.Any(e => e.Id == id);
        }

        public async Task<IActionResult> RelationshipManager(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var loggedInUser = await _context.User
                .FirstOrDefaultAsync(m => m.Email == User.Identity.Name);

            var socialNetworkContext = _context.UserRelationship
                .Include(u => u.RelatedUser)
                .Include(u => u.RelatingUser)
                .Where(u => u.RelatedUserId == loggedInUser.Id || u.RelatingUserId == loggedInUser.Id);

            var includedRelated = socialNetworkContext
                .Select(u => u.RelatedUser);

            var includedRelating = socialNetworkContext
                .Select(u => u.RelatingUser);

            ViewData["OtherUsers"] = _context.User
                .Where(u => u.Id != loggedInUser.Id)
                .Except(includedRelated)
                .Except(includedRelating).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                socialNetworkContext = _context.UserRelationship
                    .Include(u => u.RelatedUser)
                    .Include(u => u.RelatingUser)
                    .Where(u => u.RelatedUserId == loggedInUser.Id || u.RelatingUserId == loggedInUser.Id)
                    .Where(u => u.RelatingUser.FirstName.Contains(searchString) || u.RelatingUser.FirstName.Contains(searchString));

                includedRelated = socialNetworkContext
                .Select(u => u.RelatedUser);

                includedRelating = socialNetworkContext
                .Select(u => u.RelatingUser);

                ViewData["OtherUsers"] = _context.User
                    .Where(u => u.Id != loggedInUser.Id && u.FirstName.Contains(searchString))
                    .Except(includedRelated)
                    .Except(includedRelating).ToList();

            }

            return View(await socialNetworkContext.ToListAsync());
        }

        public async Task<IActionResult> Send(int id)
        {
            var loggedInUser = await _context.User
                .FirstOrDefaultAsync(m => m.Email == User.Identity.Name);
            UserRelationship userRelationship = new UserRelationship();
            userRelationship.DateOfRequest = DateTime.Now;
            userRelationship.RelatedUserId = id;
            userRelationship.RelatingUserId = loggedInUser.Id;
            userRelationship.RequestStatus = "Pending";
            userRelationship.RelationshipStatus = "Not Friend";
            if (ModelState.IsValid)
            {
                _context.Add(userRelationship);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(RelationshipManager), new { @search = ViewData["CurrentFilter"] });
        }

        public async Task<IActionResult> Cancel(int? id)
        {
            var userRelationship = await _context.UserRelationship.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                    userRelationship.RequestStatus = "Canceled";
                    _context.Update(userRelationship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRelationshipExists(userRelationship.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(RelationshipManager), new { @search = ViewData["CurrentFilter"] });

        }

        public async Task<IActionResult> Accept(int? id)
        {
            var userRelationship = await _context.UserRelationship.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                    userRelationship.RequestStatus = "Accepted";
                    userRelationship.RelationshipStatus = "Friend";
                    _context.Update(userRelationship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRelationshipExists(userRelationship.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(RelationshipManager), new { @search = ViewData["CurrentFilter"] });
        }

        public async Task<IActionResult> Decline(int? id)
        {
            var userRelationship = await _context.UserRelationship.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                    userRelationship.RequestStatus = "Declined";
                    userRelationship.RelationshipStatus = "Not Friend";
                    _context.Update(userRelationship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRelationshipExists(userRelationship.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(RelationshipManager), new { @search = ViewData["CurrentFilter"] });
        }

        public async Task<IActionResult> Resend(int? id)
        {
            var userRelationship = await _context.UserRelationship.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                    userRelationship.RequestStatus = "Pending";
                    _context.Update(userRelationship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRelationshipExists(userRelationship.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(RelationshipManager), new { @search = ViewData["CurrentFilter"] });
        }

        public async Task<IActionResult> Remove(int? id)
        {
            var userRelationship = await _context.UserRelationship.FindAsync(id);
            _context.UserRelationship.Remove(userRelationship);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RelationshipManager), ViewData["CurrentFilter"]);
        }
    }
}

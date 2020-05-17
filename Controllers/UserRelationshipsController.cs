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
    }
}

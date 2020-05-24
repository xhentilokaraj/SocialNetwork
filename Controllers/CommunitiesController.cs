using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class CommunitiesController : Controller
    {
        private readonly SocialNetworkContext _context;

        public CommunitiesController(SocialNetworkContext context)
        {
            _context = context;
        }

        // GET: Communities
        public async Task<IActionResult> Index(string currentFilter,
                                                string searchString,
                                                int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var communities = from c in _context.Community.Include(c => c.Owner)
                              select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                communities = communities.Where(s => s.CommunityName.Contains(searchString));
            }
            int pageSize = 4;
            return View(await PaginatedList<Community>.CreateAsync(communities.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Communities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var community = await _context.Community
                .Include(c => c.Owner)
                .Include(m => m.CommunityMembers).ThenInclude(u => u.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (community == null)
            {
                return NotFound();
            }

            return View(community);
        }

        // GET: Communities/Create
        public IActionResult Create()
        {
            ViewData["OwnerId"] = new SelectList(_context.User, "Id", "Email");
            return View();
        }

        // POST: Communities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CommunityName,OwnerId,CreationDate,Status")] Community community)
        {
            if (ModelState.IsValid)
            {
                _context.Add(community);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerId"] = new SelectList(_context.User, "Id", "Email", community.OwnerId);
            return View(community);
        }

        // GET: Communities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var community = await _context.Community.FindAsync(id);
            if (community == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_context.User, "Id", "Email", community.OwnerId);
            return View(community);
        }

        // POST: Communities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommunityName,OwnerId,CreationDate,Status")] Community community)
        {
            if (id != community.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(community);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunityExists(community.Id))
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
            ViewData["OwnerId"] = new SelectList(_context.User, "Id", "Email", community.OwnerId);
            return View(community);
        }

        // GET: Communities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var community = await _context.Community
                .Include(c => c.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (community == null)
            {
                return NotFound();
            }

            return View(community);
        }

        // POST: Communities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var community = await _context.Community.FindAsync(id);
            _context.Community.Remove(community);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommunityExists(int id)
        {
            return _context.Community.Any(e => e.Id == id);
        }
    }
}

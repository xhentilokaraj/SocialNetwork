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
    public class CommunityMembersController : Controller
    {
        private readonly SocialNetworkContext _context;

        public CommunityMembersController(SocialNetworkContext context)
        {
            _context = context;
        }

        // GET: CommunityMembers
        public async Task<IActionResult> Index()
        {
            var socialNetworkContext = _context.CommunityMember.Include(c => c.Community).Include(c => c.User);
            return View(await socialNetworkContext.ToListAsync());
        }

        // GET: CommunityMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communityMember = await _context.CommunityMember
                .Include(c => c.Community)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (communityMember == null)
            {
                return NotFound();
            }

            return View(communityMember);
        }

        // GET: CommunityMembers/Create
        public IActionResult Create()
        {
            ViewData["CommunityId"] = new SelectList(_context.Community, "Id", "CommunityName");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
            return View();
        }

        // POST: CommunityMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,CommunityId,DateOfJoining,Status")] CommunityMember communityMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(communityMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CommunityId"] = new SelectList(_context.Community, "Id", "CommunityName", communityMember.CommunityId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", communityMember.UserId);
            return View(communityMember);
        }

        // GET: CommunityMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communityMember = await _context.CommunityMember.FindAsync(id);
            if (communityMember == null)
            {
                return NotFound();
            }
            ViewData["CommunityId"] = new SelectList(_context.Community, "Id", "CommunityName", communityMember.CommunityId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", communityMember.UserId);
            return View(communityMember);
        }

        // POST: CommunityMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,CommunityId,DateOfJoining,Status")] CommunityMember communityMember)
        {
            if (id != communityMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(communityMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunityMemberExists(communityMember.Id))
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
            ViewData["CommunityId"] = new SelectList(_context.Community, "Id", "CommunityName", communityMember.CommunityId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", communityMember.UserId);
            return View(communityMember);
        }

        // GET: CommunityMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communityMember = await _context.CommunityMember
                .Include(c => c.Community)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (communityMember == null)
            {
                return NotFound();
            }

            return View(communityMember);
        }

        // POST: CommunityMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var communityMember = await _context.CommunityMember.FindAsync(id);
            _context.CommunityMember.Remove(communityMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommunityMemberExists(int id)
        {
            return _context.CommunityMember.Any(e => e.Id == id);
        }
    }
}

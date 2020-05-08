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
    public class SecurityQuestionsController : Controller
    {
        private readonly SocialNetworkContext _context;

        public SecurityQuestionsController(SocialNetworkContext context)
        {
            _context = context;
        }

        // GET: SecurityQuestions
        public async Task<IActionResult> Index()
        {
            return View(await _context.SecurityQuestion.ToListAsync());
        }

        // GET: SecurityQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityQuestion = await _context.SecurityQuestion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityQuestion == null)
            {
                return NotFound();
            }

            return View(securityQuestion);
        }

        // GET: SecurityQuestions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SecurityQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question")] SecurityQuestion securityQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(securityQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(securityQuestion);
        }

        // GET: SecurityQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityQuestion = await _context.SecurityQuestion.FindAsync(id);
            if (securityQuestion == null)
            {
                return NotFound();
            }
            return View(securityQuestion);
        }

        // POST: SecurityQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question")] SecurityQuestion securityQuestion)
        {
            if (id != securityQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(securityQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecurityQuestionExists(securityQuestion.Id))
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
            return View(securityQuestion);
        }

        // GET: SecurityQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityQuestion = await _context.SecurityQuestion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityQuestion == null)
            {
                return NotFound();
            }

            return View(securityQuestion);
        }

        // POST: SecurityQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var securityQuestion = await _context.SecurityQuestion.FindAsync(id);
            _context.SecurityQuestion.Remove(securityQuestion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SecurityQuestionExists(int id)
        {
            return _context.SecurityQuestion.Any(e => e.Id == id);
        }
    }
}

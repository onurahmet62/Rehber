using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rehber.Models;

namespace Rehber.Controllers
{
    public class ContactController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RehberDbContext _context;

        public ContactController(UserManager<IdentityUser> userManager, RehberDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Contact
        public async Task<IActionResult> Index()
        {
            var activeContacts = await _context.Contacts
                               .Where(c => !c.IsDeleted) // Filtering out IsDeleted = false
                               .ToListAsync();

            return View(activeContacts);
        }

        // GET: Contact/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contact/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PhoneNumber,IsDeleted,CreatedDate")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User); // Giriş yapmış olan kullanıcıyı al

                if (user != null)
                {
                    contact.Id = Guid.NewGuid();
                    contact.CreatedBy = user.UserName; // CreatedBy'nin bir kullanıcı ID'si olduğunu varsayıyoruz, gerektiğinde değiştirin
                    _context.Add(contact);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            ModelState.AddModelError("CreatedBy", "CreatedBy alanı gereklidir.");
            return View(contact);
        }



        // GET: Contact/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contact/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstName,LastName,PhoneNumber,IsDeleted,CreatedBy,CreatedDate")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
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
            return View(contact);
        }

        // GET: Contact/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                contact.IsDeleted = true; // IsDeleted değerini true yaparak soft delete işlemi gerçekleştiriyoruz.
                _context.Contacts.Update(contact); // Değişikliği kaydetmek için güncelleme yapılıyor.
                await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydetmek için SaveChangesAsync kullanılıyor.
            }

            return RedirectToAction(nameof(Index)); // Index sayfasına yönlendirme yapılıyor.
        }


        private bool ContactExists(Guid id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}

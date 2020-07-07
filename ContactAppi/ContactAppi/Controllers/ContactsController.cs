using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactAppi.Data;
using ContactAppi.Models;
using System;

namespace ContactAppi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ContactSkillContext _context;

        public ContactsController(ContactSkillContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDB>>> GetContacts()
        {
            return await _context.Contacts
                        .AsNoTracking()
                        .AsQueryable()
                        .Include(m => m.ContactSkills)
                        .ToListAsync();
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDB>> GetContact(int id)
        {
            var contact = await _context.Contacts.Include(c => c.ContactSkills).FirstOrDefaultAsync(p => p.ContactId == id);


            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, ContactDB contact)
        {
            // Get the contactDB
            //ContactDB contactDB = ContactDBToCreate(contact, _context);

            if (id != contact.ContactId)
            {
                return BadRequest();
            }

            _context.Entry(contact).State = EntityState.Modified;

            if (contact.ContactSkills != null)
            {
                foreach (var skill in contact.ContactSkills)
                {
                    _context.Entry(skill).State = EntityState.Modified;
                }
                
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        // POST: api/Contacts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            SkillDB skill;

            // Create the contact
            var contactDB = new ContactDB
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                FullName = contact.FullName,
                Address = contact.Address,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                ContactSkills = null
            };

            // Look for Skills to add
            if (contact.Skills.Count > 0)
            {
                foreach (var skilltoAdd in contact.Skills)
                {
                    skill = await _context.Skills.FindAsync(skilltoAdd.id);

                    if (skill != null)
                    {
                        ContactSkill contactSkill = new ContactSkill();
                        contactSkill.Skill = skill;
                        contactDB.ContactSkills = new List<ContactSkill>();
                        contactDB.ContactSkills.Add(contactSkill);
                    }
                }
            }

            //Save 
            _context.Contacts.Add(contactDB);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContact", new { id = contactDB.ContactId }, contactToCreate(contactDB));
        }



        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactDB>> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return contact;
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.ContactId == id);
        }

        private static Contact contactToCreate(ContactDB contactDB) =>
       new Contact
       {
           FirstName = contactDB.FirstName,
           LastName = contactDB.LastName,
           FullName = contactDB.FullName,
           Address = contactDB.Address,
           Email = contactDB.Email,
           PhoneNumber = contactDB.PhoneNumber,
       };

        private static ContactDB ContactDBToCreate(Contact contact, ContactSkillContext context)
        {
            SkillDB skill;

            // Create the contact
            var contactDB = new ContactDB
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                FullName = contact.FullName,
                Address = contact.Address,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                ContactSkills = null
            };

            // Look for Skills to add
            if (contact.Skills.Count > 0)
            {
                foreach (var skilltoAdd in contact.Skills)
                {
                    skill = context.Skills.Find(skilltoAdd.id);

                    if (skill != null)
                    {
                        ContactSkill contactSkill = new ContactSkill();
                        contactSkill.Skill = skill;
                        contactDB.ContactSkills = new List<ContactSkill>();
                        contactDB.ContactSkills.Add(contactSkill);
                    }
                }
            }

            return contactDB;
        }


    }
}

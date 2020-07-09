using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactAppi.Data;
using ContactAppi.Models;

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
        public async Task<IActionResult> PutContact(int id, Contact contact)
        {
            SkillDB skill;

            // Get the contact to update
            var contactToUpdate = await _context.Contacts.FindAsync(id);
            if (contactToUpdate == null)
            {
                return NotFound();
            }

            contactToUpdate.FirstName = contact.FirstName;
            contactToUpdate.LastName = contact.LastName;
            contactToUpdate.FullName = contact.FullName;
            contactToUpdate.Address = contact.Address;
            contactToUpdate.Email = contact.Email;
            contactToUpdate.PhoneNumber = contact.PhoneNumber;

            // Look for Skills to add
            if (contact.Skills.Count > 0)
            {
                foreach (var skilltoAdd in contact.Skills)
                {
                    skill = await _context.Skills.FindAsync(skilltoAdd.id);

                    if (skill != null)
                    {
                        ContactSkill contactSkill = new ContactSkill
                        {
                            ContactId = id,
                            Contact = contactToUpdate,
                            SkillId = skill.SkillId,
                            Skill = skill

                        };

                        contactToUpdate.ContactSkills = new List<ContactSkill>();

                        contactToUpdate.ContactSkills.Add(contactSkill);
                    }
                }
            }

            // Update the Context
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
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

            return CreatedAtAction("GetContact", new { id = contactDB.ContactId }, ContactToCreate(contactDB));
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

        private static Contact ContactToCreate(ContactDB contactDB) =>
        new Contact
        {
            FirstName = contactDB.FirstName,
            LastName = contactDB.LastName,
            FullName = contactDB.FullName,
            Address = contactDB.Address,
            Email = contactDB.Email,
            PhoneNumber = contactDB.PhoneNumber
        };




    }
}

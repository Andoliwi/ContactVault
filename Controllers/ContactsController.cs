using ContactVault.Api.Data;
using ContactVault.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactVault.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly ContactDbContext _db;

    public ContactsController(ContactDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contact>>> GetContacts([FromQuery] string? search)
    {
        var query = _db.Contacts.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(c =>
                c.FirstName.ToLower().Contains(term.ToLower()) ||
                c.LastName.ToLower().Contains(term.ToLower()));
        }

        return Ok(await query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync());
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Contact>>> Search([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return BadRequest("El parámetro 'term' es obligatorio.");
        }

        var lower = term.Trim().ToLower();

        var contacts = await _db.Contacts
            .AsNoTracking()
            .Where(c => c.FirstName.ToLower().Contains(lower) || c.LastName.ToLower().Contains(lower))
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .ToListAsync();

        return Ok(contacts);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        var contact = await _db.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

        if (contact is null)
        {
            return NotFound($"No se encontró el contacto con id {id}.");
        }

        return Ok(contact);
    }

    [HttpPost]
    public async Task<ActionResult<Contact>> CreateContact(Contact contact)
    {
        _db.Contacts.Add(contact);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateContact(int id, Contact contact)
    {
        if (id != contact.Id)
        {
            return BadRequest("El id del cuerpo no coincide con el id de la ruta.");
        }

        var existing = await _db.Contacts.FindAsync(id);

        if (existing is null)
        {
            return NotFound($"No se encontró el contacto con id {id}.");
        }

        existing.FirstName = contact.FirstName;
        existing.LastName = contact.LastName;
        existing.Phone = contact.Phone;
        existing.Email = contact.Email;
        existing.Company = contact.Company;

        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var contact = await _db.Contacts.FindAsync(id);

        if (contact is null)
        {
            return NotFound($"No se encontró el contacto con id {id}.");
        }

        _db.Contacts.Remove(contact);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
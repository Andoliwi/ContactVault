using ContactVault.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactVault.Api.Data;

public class ContactDbContext : DbContext
{
    public ContactDbContext(DbContextOptions<ContactDbContext> options)
        : base(options)
    {
    }

    public DbSet<Contact> Contacts => Set<Contact>();
}
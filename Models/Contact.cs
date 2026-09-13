using System.ComponentModel.DataAnnotations;

namespace ContactVault.Api.Models;

public class Contact
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Phone]
    [StringLength(30)]
    public string? Phone { get; set; }

    [EmailAddress]
    [StringLength(150)]
    public string? Email { get; set; }

    [StringLength(150)]
    public string? Company { get; set; }
}
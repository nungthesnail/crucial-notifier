using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Web.Site.Data.Models;

public class Subscriptition
{
    public int Id { get; set; }
    [MaxLength(64)]
    public string UserId { get; set; } = null!;
    public IdentityUser User { get; set; } = null!;
    public bool Active { get; set; }
}

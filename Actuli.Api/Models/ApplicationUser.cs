namespace Actuli.Api.Models;

public class ApplicationUser
{
    public int Id { get; set; }
    public Guid Owner { get; set; }
    public string Description { get; set; } = string.Empty;
}
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SMTG.API.Models;

[Table("roles")]
public class Role : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("code")]
    public string? Code { get; set; }

    [Column("nom")]
    public string? Nom { get; set; }

    [Column("description")]
    public string? Description { get; set; }
}
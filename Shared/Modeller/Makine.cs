using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Shared.Modeller;

[Table("Makine")]
public class Makine : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("Ad")]
    public string Ad { get; set; } = "";

    [Column("XEkseniLimiti")]
    public double XEkseniLimiti { get; set; }

    [Column("YEksenLimiti")] // Supabase'de 'i' harfi eksik yazılmış
    public double YEkseniLimiti { get; set; }

    [Column("EmniyetMesafesi")]
    public double EmniyetMesafesi { get; set; }

    [Column("Hiz")]
    public double Hiz { get; set; }
}
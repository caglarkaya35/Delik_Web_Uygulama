using Supabase.Postgrest.Attributes; // Paketinize tam uyumlu güncel adres budur

namespace Shared.Modeller;

[Table("Delik")]
public class Delik
{
    [PrimaryKey("id", false)]
    public int id { get; set; }

    [Column("X")]
    public double X { get; set; }

    [Column("Y")]
    public double Y { get; set; }

    [Column("Cap")] // Supabase'de tamamen küçük yazdýysanýz "cap" yapýn
    public double Cap { get; set; }

    [Column("Derinlik")] // Supabase'de tamamen küçük yazdýysanýz "derinlik" yapýn
    public double Derinlik { get; set; }

    [Column("Tip")] // Supabase'de tamamen küçük yazdýysanýz "tip" yapýn
    public string Tip { get; set; } = string.Empty;
}
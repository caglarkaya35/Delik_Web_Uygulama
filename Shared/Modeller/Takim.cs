using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Shared.Modeller;

[Table("Takim")]
public class Takim : BaseModel
{
    [PrimaryKey("No", false)] // Tak�m numaras�n� do�rudan birincil anahtar (Primary Key) yapt�k
    public int No { get; set; }

    [Column("Ad")] // E�er Supabase'de tamamen k���k harfse "ad" yap�n
    public string Ad { get; set; } = "";

    [Column("Tip")] // E�er Supabase'de "tip" ise g�ncelleyin
    public string Tip { get; set; } = "";

    [Column("Cap")] // E�er Supabase'de k���k harfle "cap" ise g�ncelleyin
    public double Cap { get; set; }

    [Column("XYDalmaHizi")] // E�er Supabase'de "xy_dalma_hizi" gibi yazd�ysan�z t�rnak i�ini �yle d�zeltin
    public double XYDalmaHizi { get; set; }

    [Column("ZDalmaHizi")]
    public double ZDalmaHizi { get; set; }

    [Column("XYIlerlemeHizi")]
    public double XYIlerlemeHizi { get; set; }

    [Column("ZIlerlemeHizi")]
    public double ZIlerlemeHizi { get; set; }

    [Column("XOfset")]
    public double XOfset { get; set; }

    [Column("YOfset")]
    public double YOfset { get; set; }

    [Column("ZOfset")]
    public double ZOfset { get; set; }

    [Column("Uzunluk")]
    public double Uzunluk { get; set; }

    [Column("PistonAcma")] // Pistonlar� tetikleyen M-kodlar� (�rn: "M11")
    public string PistonAcma { get; set; } = "";

    [Column("PistonKapat")] // Pistonlar� kapatan M-kodlar� (�rn: "M12")
    public string PistonKapat { get; set; } = "";
}
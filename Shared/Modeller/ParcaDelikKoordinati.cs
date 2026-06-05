using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Shared.Modeller;

[Table("ParcaDelikKoordinati")]
public class ParcaDelikKoordinati : BaseModel
{
    [PrimaryKey("id", false)] // Veritaban�ndaki otomatik artan benzersiz kimlik (Primary Key)
    public int id { get; set; }

    [Column("ParcaAdi")] // E�er Supabase'de tamamen k���k harfle "parcaadi" veya "parca_adi" yazd�ysan�z g�ncelleyin
    public string ParcaAdi { get; set; } = string.Empty;

    [Column("DelikTipi")] // E�er Supabase'de "deliktipi" veya "delik_tipi" ise ona g�re g�ncelleyin
    public string DelikTipi { get; set; } = string.Empty;

    [Column("XKoordinat")] // E�er Supabase'de "x_koordinat" veya "xkoordinat" ise g�ncelleyin
    public double XKoordinat { get; set; }

    [Column("YKoordinat")]
    public double YKoordinat { get; set; }

    [Column("ZKoordinat")]
    public double ZKoordinat { get; set; }

    [Column("Cap")] // E�er Supabase'de k���k harfle "cap" ise "cap" yap�n
    public double Cap { get; set; }

    [Column("Derinlik")] // E�er Supabase'de k���k harfle "derinlik" ise "derinlik" yap�n
    public double Derinlik { get; set; }
}
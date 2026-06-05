using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Shared.Modeller;

[Table("Birlestirme")]
public class Birlestirme : BaseModel
{
    [PrimaryKey("id", true)]
    public int Id { get; set; }

    [Column("SistemTipi")] // E�er Supabase'de tamamen k���k yazd�ysan�z "sistemtipi" veya "sistem_tipi" yap�n
    public string SistemTipi { get; set; } = "";

    [Column("KullanimAlani")] // E�er Supabase'de k���k harfse "kullanimalani" yap�n
    public string KullanimAlani { get; set; } = "";

    [Column("IslemTipi")] // E�er Supabase'de k���k harfse "islemtipi" yap�n
    public string IslemTipi { get; set; } = "";

    [Column("Capi")] // E�er Supabase'de "capi" veya "cap" ise ona g�re g�ncelleyin
    public double Capi { get; set; }

    [Column("Derinlik")]
    public double Derinlik { get; set; }

    [Column("XOfset")] // E�er Supabase'de k���k harfle "xofset" veya "x_ofset" ise buray� d�zeltin
    public double XOfset { get; set; }

    [Column("YOfset")]
    public double YOfset { get; set; }

    [Column("ZOfset")]
    public double ZOfset { get; set; }
}
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Shared.Modeller;

[Table("RafPimiAyari")]
public class RafPimiAyari : BaseModel
{
    [PrimaryKey("id", false)] // Veritabanındaki otomatik artan benzersiz kimlik (Primary Key)
    public int id { get; set; }

    [Column("DikeyAralikLimiti")] // Eğer Supabase'de tamamen küçük harfse "dikeyaraliklimiti" yapın
    public double DikeyAralikLimiti { get; set; } = 32;

    [Column("OnKenarOfset")] // Eğer Supabase'de "on_kenar_ofset" ise güncelleyin
    public double OnKenarOfset { get; set; } = 37;

    [Column("ArkaKenarOfset")] // Eğer Supabase'de "arka_kenar_ofset" ise güncelleyin
    public double ArkaKenarOfset { get; set; } = 37;

    [Column("DelikCapi")] // Eğer Supabase'de "delik_capi" veya "delikcapi" ise güncelleyin
    public double DelikCapi { get; set; } = 5;

    [Column("DelikDerinligi")] // Eğer Supabase'de "delikderinligi" ise güncelleyin
    public double DelikDerinligi { get; set; } = 12;

    [Column("YerlesimTipi")] // Eğer Supabase'de "yerlesim_tipi" ise güncelleyin
    public string YerlesimTipi { get; set; } = "Raf Konumuna Göre";

    [Column("DelikSirasiAdedi")] // Eğer Supabase'de "delik_sirasi_adedi" ise güncelleyin
    public int DelikSirasiAdedi { get; set; } = 3;
}
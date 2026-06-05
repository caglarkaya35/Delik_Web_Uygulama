using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Shared.Modeller;

[Table("Makine")]
public class Makine : BaseModel
{
    [PrimaryKey("id", false)] // Veritaban�ndaki k���k "id" ile C#'taki b�y�k "Id"yi e�le�tirdik
    public int Id { get; set; }

    [Column("Ad")] // E�er Supabase'de tamamen k���k harfle "ad" yazd�ysan�z buray� "ad" yap�n
    public string Ad { get; set; } = "";

    [Column("XEkseniLimiti")] // E�er Supabase'de "x_ekseni_limiti" veya "xeksenilimiti" ise ona g�re g�ncelleyin
    public double XEkseniLimiti { get; set; }

    [Column("YEkseniLimiti")] // E�er Supabase'de "y_ekseni_limiti" veya "yeksenilimiti" ise ona g�re g�ncelleyin
    public double YEkseniLimiti { get; set; }

    [Column("EmniyetMesafesi")] // E�er Supabase'de "emniyet_mesafesi" veya "emniyetmesafesi" ise g�ncelleyin
    public double EmniyetMesafesi { get; set; }

    [Column("Hiz")] // E�er Supabase'de k���k harfle "hiz" ise buray� "hiz" yap�n
    public double Hiz { get; set; }
}
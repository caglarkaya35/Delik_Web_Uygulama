namespace Shared.Modeller;

public class Birlestirme
{
    public int Id { get; set; }
    public string SistemTipi { get; set; } = "";
    public string KullanimAlani { get; set; } = "";
    public string IslemTipi { get; set; } = "";
    public double Capi { get; set; }
    public double Derinlik { get; set; }
    public double XOfset { get; set; }
    public double YOfset { get; set; }
    public double ZOfset { get; set; }
}

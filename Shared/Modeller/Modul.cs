namespace Shared.Modeller;

public class Modul
{
    public double En { get; set; }
    public double Boy { get; set; }
    public double Yukseklik { get; set; }
    public List<Parca> Parcalar { get; set; } = new();
}

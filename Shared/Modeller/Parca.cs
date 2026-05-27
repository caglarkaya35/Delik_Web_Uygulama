namespace Shared.Modeller;

public class Parca
{
    public string Ad { get; set; } = string.Empty;
    public double En { get; set; }
    public double Boy { get; set; }
    public double Kalinlik { get; set; }
    public List<Delik> Delikler { get; set; } = new();
    public string? Barkod { get; set; }
}

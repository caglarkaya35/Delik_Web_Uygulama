using System.Text;
using Shared.Modeller;

namespace Shared.Servisler;

public class GKodUretici
{
    public string GKodUret(Parca parca)
    {
        var sb = new StringBuilder();
        sb.AppendLine("G21"); // mm
        sb.AppendLine("G90"); // mutlak konum

        foreach (var delik in parca.Delikler)
        {
            sb.AppendLine($"G0 X{delik.X:F2} Y{delik.Y:F2}");
            sb.AppendLine($"G1 Z-{delik.Derinlik:F2}");
            sb.AppendLine("G0 Z5");
        }

        sb.AppendLine("M30");
        return sb.ToString();
    }

    public string DosyaAdiOlustur(Parca parca)
    {
        var tarih = DateTime.Now.ToString("yyyyMMdd");
        return $"{tarih}_{parca.Ad}.cnc";
    }
}

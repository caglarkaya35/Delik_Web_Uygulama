using System.Globalization;
using System.Text;
using Shared.Modeller;

namespace Shared.Servisler;

public class GKodUretici
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    public string GKodUret(Parca parca, List<Takim> takimlar, List<Birlestirme> birlestirmeler)
    {
        var sb = new StringBuilder();
        int n = 10;

        void Yaz(string komut)
        {
            sb.AppendLine($"N{n} {komut}");
            n += 10;
        }

        // BAŞLIK
        Yaz("G90");
        Yaz("M318 (FREZE MOTORUNU ÇALIŞTIR)");

        double yarimEn = parca.En / 2.0;
        double yarimBoy = parca.Boy / 2.0;

        // Köşe sırası: Sol Alt → Sağ Alt → Sol Üst → Sağ Üst
        (bool sol, bool alt)[] koseler =
        {
            (true,  true ),
            (false, true ),
            (true,  false),
            (false, false),
        };

        foreach (var (sol, alt) in koseler)
        {
            var koseDelikleri = parca.Delikler
                .Where(d =>
                    (sol ? d.X <= yarimEn : d.X > yarimEn) &&
                    (alt ? d.Y <= yarimBoy : d.Y > yarimBoy))
                .ToList();

            if (!koseDelikleri.Any()) continue;

            // --- ÇANAK ---
            var canak = koseDelikleri.FirstOrDefault(d => d.Tip == "C");
            if (canak != null)
            {
                var t = TakimBul("ÇANAK", takimlar);
                var b = BirlestirmeBul("ÇANAK", birlestirmeler);
                if (t != null && b != null)
                {
                    double x = canak.Y + t.XOfset;
                    double y = canak.X + t.YOfset;
                    double z = t.ZOfset;
                    Yaz($"G1 X{F(x)} Y{F(y)} Z{F(z)} F{F0(t.XYIlerlemeHizi)}");
                    Yaz(t.PistonAcma);
                    Yaz($"G1 Z{F(z - b.Derinlik)} F{F0(t.ZDalmaHizi)}");
                    Yaz($"G1 Z{F(z)} F{F0(t.ZIlerlemeHizi)}");
                    Yaz(t.PistonKapat);
                }
            }

            // --- YATAY MİL ---
            var yatayMil = koseDelikleri.FirstOrDefault(d => d.Tip == "M");
            if (yatayMil != null)
            {
                string takimTipi = sol
                    ? (alt ? "Y+SOL(1)" : "Y-SOL(3)")
                    : (alt ? "Y+SAĞ(2)" : "Y-SAĞ(4)");

                var t = TakimBul(takimTipi, takimlar);
                var tSol = TakimBul("Y+SOL(1)", takimlar);
                var tSag = TakimBul("Y+SAĞ(2)", takimlar);
                var b = BirlestirmeBul("YATAY MİL", birlestirmeler);

                if (t != null && b != null)
                {
                    double x = t.XOfset + yatayMil.Y;
                    double y = t.YOfset + yatayMil.X;
                    double z = t.ZOfset - (parca.Kalinlik / 2.0 + t.Cap / 2.0);

                    Yaz($"G1 X{F(x)} Y{F(y)} Z{F(z)} F{F0(t.XYIlerlemeHizi)}");

                    if (sol)
                    {
                        if (tSol != null) Yaz(tSol.PistonAcma);
                        if (tSag != null) Yaz(tSag.PistonAcma);
                    }
                    else
                    {
                        if (tSag != null) Yaz(tSag.PistonAcma);
                        if (tSol != null) Yaz(tSol.PistonAcma);
                    }

                    double yeni = alt ? y + b.Derinlik : y - b.Derinlik;

                    Yaz($"G1 Y{F(yeni)} F{F0(t.ZDalmaHizi)}");
                    Yaz($"G1 Y{F(y)} F{F0(t.ZIlerlemeHizi)}");

                    if (sol)
                    {
                        if (tSol != null) Yaz(tSol.PistonKapat);
                        if (tSag != null) Yaz(tSag.PistonKapat);
                    }
                    else
                    {
                        if (tSag != null) Yaz(tSag.PistonKapat);
                        if (tSol != null) Yaz(tSol.PistonKapat);
                    }
                }
            }

            // --- YATAY DÜBEL ---
            var yatayDubel = koseDelikleri.FirstOrDefault(d => d.Tip == "D");
            if (yatayDubel != null)
            {
                string takimTipi = sol
                    ? (alt ? "Y+SOL(1)" : "Y-SOL(3)")
                    : (alt ? "Y+SAĞ(2)" : "Y-SAĞ(4)");

                var t = TakimBul(takimTipi, takimlar);
                var tSol = TakimBul("Y+SOL(1)", takimlar);
                var tSag = TakimBul("Y+SAĞ(2)", takimlar);
                var b = BirlestirmeBul("YATAY DÜBEL", birlestirmeler);

                if (t != null && b != null)
                {
                    double x = t.XOfset + yatayDubel.Y;
                    double y = t.YOfset + yatayDubel.X;
                    double z = t.ZOfset - (parca.Kalinlik / 2.0 + t.Cap / 2.0);

                    Yaz($"G1 X{F(x)} Y{F(y)} Z{F(z)} F{F0(t.XYIlerlemeHizi)}");

                    if (sol)
                    {
                        if (tSol != null) Yaz(tSol.PistonAcma);
                        if (tSag != null) Yaz(tSag.PistonAcma);
                    }
                    else
                    {
                        if (tSag != null) Yaz(tSag.PistonAcma);
                        if (tSol != null) Yaz(tSol.PistonAcma);
                    }

                    double yeni = alt ? y + b.Derinlik : y - b.Derinlik;

                    Yaz($"G1 Y{F(yeni)} F{F0(t.ZDalmaHizi)}");
                    Yaz($"G1 Y{F(y)} F{F0(t.ZIlerlemeHizi)}");

                    if (sol)
                    {
                        if (tSol != null) Yaz(tSol.PistonKapat);
                        if (tSag != null) Yaz(tSag.PistonKapat);
                    }
                    else
                    {
                        if (tSag != null) Yaz(tSag.PistonKapat);
                        if (tSol != null) Yaz(tSol.PistonKapat);
                    }
                }
            }

            // --- YÜZEY MİL + YÜZEY DÜBEL (tek blok, tek hareket) ---
            var yuzeyMil = koseDelikleri.FirstOrDefault(d => d.Tip == "SM");
            var yuzeyDubel = koseDelikleri.FirstOrDefault(d => d.Tip == "SD");

            if (yuzeyMil != null || yuzeyDubel != null)
            {
                var tSM = TakimBul("YÜZEY MİL", takimlar);
                var tSD = TakimBul("YÜZEY DÜBEL", takimlar);
                var bSM = BirlestirmeBul("YÜZEY MİL", birlestirmeler);
                var bSD = BirlestirmeBul("YÜZEY DÜBEL", birlestirmeler);

                // Sol köşe → YÜZEY MİL takım offseti, Sağ köşe → YÜZEY DÜBEL takım offseti
                Takim? tOfset = sol ? tSM : tSD;
                Delik? dKoord = yuzeyMil ?? yuzeyDubel;

                if (tOfset != null && dKoord != null)
                {
                    double x = tOfset.XOfset + dKoord.Y;
                    double y = tOfset.YOfset + dKoord.X;
                    double z = tOfset.ZOfset;

                    Yaz($"G1 X{F(x)} Y{F(y)} Z{F(z)} F{F0(tOfset.XYIlerlemeHizi)}");

                    if (tSM != null) Yaz(tSM.PistonAcma);   // M310
                    if (tSD != null) Yaz(tSD.PistonAcma);   // M311

                    double derinlik = (yuzeyMil != null && bSM != null)
                        ? bSM.Derinlik
                        : (bSD?.Derinlik ?? 0);

                    Yaz($"G1 Z{F(z - derinlik)} F{F0(tOfset.ZDalmaHizi)}");
                    Yaz($"G1 Z{F(z)} F{F0(tOfset.ZIlerlemeHizi)}");

                    if (tSM != null) Yaz(tSM.PistonKapat);  // M410
                    if (tSD != null) Yaz(tSD.PistonKapat);  // M411
                }
            }
        }

        // SON BÖLÜM
        Yaz("M418 (FREZEYİ KAPAT)");
        Yaz("G0 Z0 X0 Y1500");
        Yaz("M30 (PROGRAM SONU)");

        return sb.ToString();
    }

    private static Takim? TakimBul(string tip, List<Takim> takimlar) =>
        takimlar.FirstOrDefault(t => t.Tip == tip);

    private static Birlestirme? BirlestirmeBul(string islemTipi, List<Birlestirme> birlestirmeler) =>
        birlestirmeler.FirstOrDefault(b => b.IslemTipi == islemTipi);

    private static string F(double v) => v.ToString("F3", Inv);
    private static string F0(double v) => v.ToString("F0", Inv);

    public string DosyaAdiOlustur(Parca parca)
    {
        var tarih = DateTime.Now.ToString("yyyyMMdd");
        return $"{tarih}_{parca.Ad}.cnc";
    }
}

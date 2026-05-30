using Shared.Modeller;

namespace Shared.Servisler;

public class DelikHesaplama
{
    public void DelikleriHesapla(Parca parca, List<Birlestirme> birlestirmeler, string sistemTipi, string panelRolü, string montajTipi, double derinlik)
    {
        parca.Delikler.Clear();
        if (sistemTipi != "MİNİFİKS") return;

        var c = birlestirmeler.FirstOrDefault(x => x.SistemTipi == "MİNİFİKS" && x.IslemTipi == "ÇANAK") ?? new Birlestirme { XOfset = 9.5, YOfset = 37, Capi = 15, Derinlik = 12.5 };
        var ym = birlestirmeler.FirstOrDefault(x => x.SistemTipi == "MİNİFİKS" && x.IslemTipi == "YATAY MİL") ?? new Birlestirme { XOfset = 9.5, YOfset = 82, Capi = 8, Derinlik = 10 };
        var yd = birlestirmeler.FirstOrDefault(x => x.SistemTipi == "MİNİFİKS" && x.IslemTipi == "YATAY DÜBEL") ?? new Birlestirme { XOfset = 9.5, YOfset = 128, Capi = 8, Derinlik = 10 };
        var sm = birlestirmeler.FirstOrDefault(x => x.SistemTipi == "MİNİFİKS" && x.IslemTipi == "YÜZEY MİL") ?? new Birlestirme { XOfset = 9.5, YOfset = 82, Capi = 8, Derinlik = 10 };
        var sd = birlestirmeler.FirstOrDefault(x => x.SistemTipi == "MİNİFİKS" && x.IslemTipi == "YÜZEY DÜBEL") ?? new Birlestirme { XOfset = 9.5, YOfset = 128, Capi = 8, Derinlik = 10 };

        // Her delik tipi kendi Birleştirme satırındaki XOfset ve YOfset değerlerini kullanır.
        // XOfset → parça yüzeyinde Y konumu (kenardan ne kadar içeride)
        // YOfset → Z ekseni boyunca önden arkaya konum
        // Bir kenar boyunca delik grubu ekler. Delikler derinlik (En) ekseni boyunca
        // önden arkaya simetrik dizilir; ön yarı listelenir, arka yarı aynalanır.
        void EkleGrup(bool isSecondSide, string grupTipi)
        {
            double GetBoyOfs(Birlestirme b) => isSecondSide ? parca.Boy - b.XOfset : b.XOfset;

            switch (grupTipi)
            {
                case "kenar":
                    // Önden arkaya: Ç -> YtM -> YtD -> YtD -> YtM -> Ç
                    AddH(c.YOfset, GetBoyOfs(c), c, "C");
                    AddH(ym.YOfset, GetBoyOfs(ym), ym, "M");
                    AddH(yd.YOfset, GetBoyOfs(yd), yd, "D");
                    AddH(parca.En - yd.YOfset, GetBoyOfs(yd), yd, "D");
                    AddH(parca.En - ym.YOfset, GetBoyOfs(ym), ym, "M");
                    AddH(parca.En - c.YOfset, GetBoyOfs(c), c, "C");
                    break;

                case "yuzey":
                    // Önden arkaya: YzM -> YzD -> YzD -> YzM
                    AddH(sm.YOfset, GetBoyOfs(sm), sm, "SM");
                    AddH(sd.YOfset, GetBoyOfs(sd), sd, "SD");
                    AddH(parca.En - sd.YOfset, GetBoyOfs(sd), sd, "SD");
                    AddH(parca.En - sm.YOfset, GetBoyOfs(sm), sm, "SM");
                    break;

                case "yuzeyKose":
                    // Sol köşe + sağ köşe simetrisi: YzM, YzD yan yana (Boy ekseninde)
                    AddH(sm.XOfset, isSecondSide ? parca.Boy - sm.YOfset : sm.YOfset, sm, "SM");
                    AddH(sd.XOfset, isSecondSide ? parca.Boy - sd.YOfset : sd.YOfset, sd, "SD");
                    AddH(parca.En - sm.XOfset, isSecondSide ? parca.Boy - sm.YOfset : sm.YOfset, sm, "SM");
                    AddH(parca.En - sd.XOfset, isSecondSide ? parca.Boy - sd.YOfset : sd.YOfset, sd, "SD");
                    break;

                case "canakYuzey":
                    // Sol köşe + sağ köşe simetrisi: Ç, YzM, YzD
                    AddH(c.XOfset,             isSecondSide ? parca.Boy - c.YOfset : c.YOfset, c,  "C");
                    AddH(sm.XOfset,            isSecondSide ? parca.Boy - sm.YOfset : sm.YOfset, sm, "SM");
                    AddH(sd.XOfset,            isSecondSide ? parca.Boy - sd.YOfset : sd.YOfset, sd, "SD");
                    AddH(parca.En - c.XOfset,  isSecondSide ? parca.Boy - c.YOfset : c.YOfset, c,  "C");
                    AddH(parca.En - sm.XOfset, isSecondSide ? parca.Boy - sm.YOfset : sm.YOfset, sm, "SM");
                    AddH(parca.En - sd.XOfset, isSecondSide ? parca.Boy - sd.YOfset : sd.YOfset, sd, "SD");
                    break;

                case "yatay":
                    // Sol köşe + sağ köşe simetrisi: YtM, YtD
                    AddH(ym.XOfset,            isSecondSide ? parca.Boy - ym.YOfset : ym.YOfset, ym, "M");
                    AddH(yd.XOfset,            isSecondSide ? parca.Boy - yd.YOfset : yd.YOfset, yd, "D");
                    AddH(parca.En - ym.XOfset, isSecondSide ? parca.Boy - ym.YOfset : ym.YOfset, ym, "M");
                    AddH(parca.En - yd.XOfset, isSecondSide ? parca.Boy - yd.YOfset : yd.YOfset, yd, "D");
                    break;

                case "canakYatay":
                    // Sol köşe + sağ köşe simetrisi: Ç, YtM, YtD
                    AddH(c.XOfset,             isSecondSide ? parca.Boy - c.YOfset : c.YOfset, c,  "C");
                    AddH(ym.XOfset,            isSecondSide ? parca.Boy - ym.YOfset : ym.YOfset, ym, "M");
                    AddH(yd.XOfset,            isSecondSide ? parca.Boy - yd.YOfset : yd.YOfset, yd, "D");
                    AddH(parca.En - c.XOfset,  isSecondSide ? parca.Boy - c.YOfset : c.YOfset, c,  "C");
                    AddH(parca.En - ym.XOfset, isSecondSide ? parca.Boy - ym.YOfset : ym.YOfset, ym, "M");
                    AddH(parca.En - yd.XOfset, isSecondSide ? parca.Boy - yd.YOfset : yd.YOfset, yd, "D");
                    break;
            }
        }

        void AddH(double enKonum, double boyKonum, Birlestirme b, string tip)
        {
            parca.Delikler.Add(new Delik {
                X = enKonum,
                Y = boyKonum,
                Cap = b.Capi,
                Derinlik = b.Derinlik,
                Tip = tip
            });
        }

        // Genel kural:
        //   - Parçanın bir ucu karşı parçanın İÇİNE giriyorsa → o uçta KENAR deliği (Ç-YtM-YtD)
        //   - Parçanın bir ucu karşı parçanın YÜZEYİNE oturuyorsa → o uçta YÜZEY deliği (YzM-YzD)
        // ALT/ÜST: iki ucu da aynı role sahip (her ikisi de SOL ve SAĞ ile aynı şekilde buluşur)
        // SOL/SAĞ: alt ucu ALT panele, üst ucu ÜST panele bağlanır → karışık montaj tiplerinde iki uç farklı olabilir
        bool altIceride = montajTipi == "Alt-Üst İçeride" || montajTipi == "Alt İçeride-Üst Dışarıda";
        bool ustIceride = montajTipi == "Alt-Üst İçeride" || montajTipi == "Alt Dışarıda-Üst İçeride";

        // first  = parçanın Boy=0 ucu (SOL/SAĞ için alt kenar)
        // second = parçanın Boy=parca.Boy ucu (SOL/SAĞ için üst kenar)
        string grupFirst, grupSecond;

        if (panelRolü == "ALT")
        {
            // ALT içerideyse iki ucu da SOL/SAĞ içine girer → kenar; dışarıdaysa yüzey
            grupFirst = grupSecond = altIceride ? "kenar" : "yuzey";
        }
        else if (panelRolü == "ÜST")
        {
            grupFirst = grupSecond = ustIceride ? "kenar" : "yuzey";
        }
        else if (panelRolü == "RAF" || panelRolü == "DİKME")
        {
            // Raf yanların, dikme ALT/ÜST arasına sıkışır → iki uç da kenar
            grupFirst = grupSecond = "kenar";
        }
        else // SOL, SAĞ
        {
            if (montajTipi == "Alt Dışarıda-Üst İçeride")
            {
                grupFirst  = "canakYuzey"; // alt kenar: Ç + YzM + YzD
                grupSecond = "yatay";      // üst kenar: YtM + YtD
            }
            else if (montajTipi == "Alt İçeride-Üst Dışarıda")
            {
                grupFirst  = "yuzeyKose"; // alt kenar: YzM + YzD köşelerde yan yana
                grupSecond = "canakYatay"; // üst kenar: Ç + YtM + YtD
            }
            else
            {
                // Saf montaj: bir uç karşı yüzeye oturuyorsa yüzey, içine giriyorsa kenar
                grupFirst = altIceride ? "yuzey" : "kenar";
                grupSecond = ustIceride ? "yuzey" : "kenar";
            }
        }

        EkleGrup(false, grupFirst);
        EkleGrup(true, grupSecond);
    }
}

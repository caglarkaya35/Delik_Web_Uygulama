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
        void EkleGrup(bool isSecondSide, bool kenarMi)
        {
            double GetBoyOfs(Birlestirme b) => isSecondSide ? parca.Boy - b.XOfset : b.XOfset;

            if (kenarMi)
            {
                // Z ekseni boyunca önden arkaya: C -> M -> D -> D -> M -> C
                AddH(c.YOfset, GetBoyOfs(c), c, "C");
                AddH(ym.YOfset, GetBoyOfs(ym), ym, "M");
                AddH(yd.YOfset, GetBoyOfs(yd), yd, "D");
                AddH(parca.En - yd.YOfset, GetBoyOfs(yd), yd, "D");
                AddH(parca.En - ym.YOfset, GetBoyOfs(ym), ym, "M");
                AddH(parca.En - c.YOfset, GetBoyOfs(c), c, "C");
            }
            else
            {
                // Z ekseni boyunca önden arkaya: SM -> SD -> SD -> SM
                AddH(sm.YOfset, GetBoyOfs(sm), sm, "SM");
                AddH(sd.YOfset, GetBoyOfs(sd), sd, "SD");
                AddH(parca.En - sd.YOfset, GetBoyOfs(sd), sd, "SD");
                AddH(parca.En - sm.YOfset, GetBoyOfs(sm), sm, "SM");
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
        //   - Parçanın bir ucu karşı parçanın İÇİNE giriyorsa → o uçta KENAR deliği (C-M-D)
        //   - Parçanın bir ucu karşı parçanın YÜZEYİNE oturuyorsa → o uçta YÜZEY deliği (SM-SD)
        // ALT/ÜST: iki ucu da aynı role sahip (her ikisi de SOL ve SAĞ ile aynı şekilde buluşur)
        // SOL/SAĞ: alt ucu ALT panele, üst ucu ÜST panele bağlanır → karışık montaj tiplerinde iki uç farklı olabilir
        bool altIceride = montajTipi == "Alt-Üst İçeride" || montajTipi == "Alt İçeride-Üst Dışarıda";
        bool ustIceride = montajTipi == "Alt-Üst İçeride" || montajTipi == "Alt Dışarıda-Üst İçeride";

        bool kenarFirst, kenarSecond;

        if (panelRolü == "ALT")
        {
            // ALT içerideyse iki ucu da SOL/SAĞ içine girer → kenar; dışarıdaysa yüzey
            kenarFirst = kenarSecond = altIceride;
        }
        else if (panelRolü == "ÜST")
        {
            kenarFirst = kenarSecond = ustIceride;
        }
        else if (panelRolü == "RAF")
        {
            // Raf her zaman yanların arasına sıkışır → iki uç da kenar
            kenarFirst = kenarSecond = true;
        }
        else if (panelRolü == "DİKME")
        {
            // Dikme her zaman ALT ile ÜST arasına sıkışır → iki uç da kenar
            kenarFirst = kenarSecond = true;
        }
        else // SOL, SAĞ
        {
            // first = alt-uç: ALT içerideyse SOL'un alt yüzeyine oturur → yüzey;
            //                 ALT dışarıdaysa SOL'un alt kenarı ALT yüzeyine girer → kenar
            // second = üst-uç: aynı mantık ÜST için
            kenarFirst = !altIceride;
            kenarSecond = !ustIceride;
        }

        EkleGrup(false, kenarFirst);
        EkleGrup(true, kenarSecond);
    }
}

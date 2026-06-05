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

        // X = parça üzerindeki En ekseni konumu (soldan sağa)
        // Y = parça üzerindeki Boy ekseni konumu (önden arkaya / kenardan kenar)
        void AddH(double x, double y, Birlestirme b, string tip)
        {
            parca.Delikler.Add(new Delik
            {
                X = x,
                Y = y,
                Cap = b.Capi,
                Derinlik = b.Derinlik,
                Tip = tip
            });
        }

        void EkleGrup(bool isSecondSide, string grupTipi)
        {
            // isSecondSide=false → Boy=0 (ön kenar), isSecondSide=true → Boy=parca.Boy (arka kenar)
            double BoyPos(Birlestirme b) => isSecondSide ? parca.Boy - b.XOfset : b.XOfset;

            switch (grupTipi)
            {
                case "kenar":
                    // Önden arkaya: Ç → YtM → YtD → YtD → YtM → Ç
                    AddH(c.YOfset, BoyPos(c), c, "C");
                    AddH(ym.YOfset, BoyPos(ym), ym, "M");
                    AddH(yd.YOfset, BoyPos(yd), yd, "D");
                    AddH(parca.En - yd.YOfset, BoyPos(yd), yd, "D");
                    AddH(parca.En - ym.YOfset, BoyPos(ym), ym, "M");
                    AddH(parca.En - c.YOfset, BoyPos(c), c, "C");
                    break;

                case "yuzey":
                    // Önden arkaya: YzM → YzD → YzD → YzM
                    AddH(sm.YOfset, BoyPos(sm), sm, "SM");
                    AddH(sd.YOfset, BoyPos(sd), sd, "SD");
                    AddH(parca.En - sd.YOfset, BoyPos(sd), sd, "SD");
                    AddH(parca.En - sm.YOfset, BoyPos(sm), sm, "SM");
                    break;

                case "yuzeyKose":
                    // Köşelerde YzM ve YzD yan yana
                    AddH(sm.XOfset, isSecondSide ? parca.Boy - sm.YOfset : sm.YOfset, sm, "SM");
                    AddH(sd.XOfset, isSecondSide ? parca.Boy - sd.YOfset : sd.YOfset, sd, "SD");
                    AddH(parca.En - sm.XOfset, isSecondSide ? parca.Boy - sm.YOfset : sm.YOfset, sm, "SM");
                    AddH(parca.En - sd.XOfset, isSecondSide ? parca.Boy - sd.YOfset : sd.YOfset, sd, "SD");
                    break;

                case "canakYuzey":
                    // Köşelerde Ç + YzM + YzD
                    AddH(c.XOfset, isSecondSide ? parca.Boy - c.YOfset : c.YOfset, c, "C");
                    AddH(sm.XOfset, isSecondSide ? parca.Boy - sm.YOfset : sm.YOfset, sm, "SM");
                    AddH(sd.XOfset, isSecondSide ? parca.Boy - sd.YOfset : sd.YOfset, sd, "SD");
                    AddH(parca.En - c.XOfset, isSecondSide ? parca.Boy - c.YOfset : c.YOfset, c, "C");
                    AddH(parca.En - sm.XOfset, isSecondSide ? parca.Boy - sm.YOfset : sm.YOfset, sm, "SM");
                    AddH(parca.En - sd.XOfset, isSecondSide ? parca.Boy - sd.YOfset : sd.YOfset, sd, "SD");
                    break;

                case "yatay":
                    // Köşelerde YtM + YtD
                    AddH(ym.XOfset, isSecondSide ? parca.Boy - ym.YOfset : ym.YOfset, ym, "M");
                    AddH(yd.XOfset, isSecondSide ? parca.Boy - yd.YOfset : yd.YOfset, yd, "D");
                    AddH(parca.En - ym.XOfset, isSecondSide ? parca.Boy - ym.YOfset : ym.YOfset, ym, "M");
                    AddH(parca.En - yd.XOfset, isSecondSide ? parca.Boy - yd.YOfset : yd.YOfset, yd, "D");
                    break;

                case "canakYatay":
                    // Köşelerde Ç + YtM + YtD
                    AddH(c.XOfset, isSecondSide ? parca.Boy - c.YOfset : c.YOfset, c, "C");
                    AddH(ym.XOfset, isSecondSide ? parca.Boy - ym.YOfset : ym.YOfset, ym, "M");
                    AddH(yd.XOfset, isSecondSide ? parca.Boy - yd.YOfset : yd.YOfset, yd, "D");
                    AddH(parca.En - c.XOfset, isSecondSide ? parca.Boy - c.YOfset : c.YOfset, c, "C");
                    AddH(parca.En - ym.XOfset, isSecondSide ? parca.Boy - ym.YOfset : ym.YOfset, ym, "M");
                    AddH(parca.En - yd.XOfset, isSecondSide ? parca.Boy - yd.YOfset : yd.YOfset, yd, "D");
                    break;
            }
        }

        bool altIceride = montajTipi == "Alt-Üst İçeride" || montajTipi == "Alt İçeride-Üst Dışarıda";
        bool ustIceride = montajTipi == "Alt-Üst İçeride" || montajTipi == "Alt Dışarıda-Üst İçeride";

        string grupFirst, grupSecond;

        if (panelRolü == "ALT")
        {
            grupFirst = grupSecond = altIceride ? "kenar" : "yuzey";
        }
        else if (panelRolü == "ÜST")
        {
            grupFirst = grupSecond = ustIceride ? "kenar" : "yuzey";
        }
        else if (panelRolü == "RAF" || panelRolü == "DİKME")
        {
            grupFirst = grupSecond = "kenar";
        }
        else // SOL, SAĞ
        {
            if (montajTipi == "Alt Dışarıda-Üst İçeride")
            {
                grupFirst = "canakYatay";
                grupSecond = "yuzeyKose";
            }
            else if (montajTipi == "Alt İçeride-Üst Dışarıda")
            {
                grupFirst = "yuzeyKose";
                grupSecond = "canakYatay";
            }
            else
            {
                grupFirst = altIceride ? "yuzey" : "kenar";
                grupSecond = ustIceride ? "yuzey" : "kenar";
            }
        }

        EkleGrup(false, grupFirst);
        EkleGrup(true, grupSecond);
    }
}
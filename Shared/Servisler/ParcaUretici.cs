using Shared.Modeller;

namespace Shared.Servisler;

public class ParcaUretici
{
    public List<Parca> ParcalariOlustur(Modul modul)
    {
        return new List<Parca>
        {
            new Parca { Ad = "sol_yan",   En = modul.Yukseklik, Boy = modul.Boy },
            new Parca { Ad = "sag_yan",   En = modul.Yukseklik, Boy = modul.Boy },
            new Parca { Ad = "ust_tabla", En = modul.En,        Boy = modul.Boy },
            new Parca { Ad = "alt_tabla", En = modul.En,        Boy = modul.Boy },
        };
    }
}

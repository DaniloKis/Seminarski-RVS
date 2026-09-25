using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    public class PaketListaKlasa
    {

        // atributi
        private List<PaketKlasa> _listaPaketa;

        // property
        public List<PaketKlasa> ListaPaketa
        {
            get
            {
                return _listaPaketa;
            }
            set
            {
                if (this._listaPaketa != value)
                    this._listaPaketa = value;
            }
        }

        // konstruktor
        public PaketListaKlasa()
        {
            _listaPaketa = new List<PaketKlasa>();

        }

        // privatne metode

        // javne metode
        public void DodajElementListe(PaketKlasa noviPaketObjekat)
        {
            _listaPaketa.Add(noviPaketObjekat);
        }

        public void ObrisiElementListe(PaketKlasa paketObjekatZaBrisanje)
        {
            _listaPaketa.Remove(paketObjekatZaBrisanje);
        }

        public void ObrisiElementNaPoziciji(int pozicija)
        {
            _listaPaketa.RemoveAt(pozicija);
        }

        public void IzmeniElementListe(PaketKlasa stariPaketObjekat, PaketKlasa noviPaketObjekat)
        {
            int indexStarogPaketa = 0;
            indexStarogPaketa = _listaPaketa.IndexOf(stariPaketObjekat);
            _listaPaketa.RemoveAt(indexStarogPaketa);
            _listaPaketa.Insert(indexStarogPaketa, noviPaketObjekat);
        }


    }
}

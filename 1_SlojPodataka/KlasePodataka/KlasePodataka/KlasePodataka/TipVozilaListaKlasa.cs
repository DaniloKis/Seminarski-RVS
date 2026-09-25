using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    public class TipVozilaListaKlasa
    {
        // atributi
        private List<TipVozilaKlasa> _listaVozila;

        // property
        public List<TipVozilaKlasa> ListaVozila
        {
            get
            {
                return _listaVozila;
            }
            set
            {
                if (this._listaVozila != value)
                    this._listaVozila = value;
            }
        }

        // konstruktor
        public TipVozilaListaKlasa()
        {
            _listaVozila = new List<TipVozilaKlasa>();

        }

        // privatne metode

        // javne metode
        public void DodajElementListe(TipVozilaKlasa novoVoziloObjekat)
        {
            _listaVozila.Add(novoVoziloObjekat);
        }

        public void ObrisiElementListe(TipVozilaKlasa voziloObjekatZaBrisanje)
        {
            _listaVozila.Remove(voziloObjekatZaBrisanje);
        }

        public void ObrisiElementNaPoziciji(int pozicija)
        {
            _listaVozila.RemoveAt(pozicija);
        }

        public void IzmeniElementListe(TipVozilaKlasa staroVoziloObjekat, TipVozilaKlasa novoVoziloObjekat)
        {
            int indexStarogVozila = 0;
            indexStarogVozila = _listaVozila.IndexOf(staroVoziloObjekat);
            _listaVozila.RemoveAt(indexStarogVozila);
            _listaVozila.Insert(indexStarogVozila, novoVoziloObjekat);
        }

    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KorisnickiInterfejs.ViewModels
{
    // DETALJNA STAVKA (koristi se u master-detail formi za unos).
    // Bez obaveznih (Required) atributa: prazni redovi se u kontroleru
    // preskacu, pa nepopunjen detalj red ne blokira snimanje paketa.
    public class StavkaUnosViewModel
    {
        [StringLength(100)]
        public string NazivArtikla { get; set; }

        public int Kolicina { get; set; }

        public decimal TezinaStavke { get; set; }
    }

    // MASTER + DETALJI (VIEW MODEL ZA PREZENTACIONU LOGIKU).
    // Objedinjuje podatke o paketu (master) i listu stavki (detalj),
    // koji se unose zajedno i snimaju u JEDNOJ transakciji.
    public class PaketUnosViewModel
    {
        // --- MASTER: podaci o paketu ---
        [Required(ErrorMessage = "Kod paketa je obavezan.")]
        [StringLength(13)]
        [Display(Name = "Kod paketa")]
        public string KodPaketa { get; set; }

        [Required(ErrorMessage = "Ime posiljaoca je obavezno.")]
        [Display(Name = "Posiljalac - ime")]
        public string PosiljalacIme { get; set; }

        [Required]
        [Display(Name = "Posiljalac - adresa")]
        public string PosiljalacAdresa { get; set; }

        [Required]
        [Display(Name = "Posiljalac - grad")]
        public string PosiljalacGrad { get; set; }

        [Required(ErrorMessage = "Ime primaoca je obavezno.")]
        [Display(Name = "Primalac - ime")]
        public string PrimalacIme { get; set; }

        [Required]
        [Display(Name = "Primalac - adresa")]
        public string PrimalacAdresa { get; set; }

        [Required]
        [Display(Name = "Primalac - postanski broj")]
        public string PrimalacPostanskiBroj { get; set; }

        [Required]
        [Display(Name = "Primalac - grad")]
        public string PrimalacGrad { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Tezina mora biti broj veci ili jednak 0.")]
        [Display(Name = "Tezina (kg)")]
        public decimal Tezina { get; set; }

        [Display(Name = "Oznaka upozorenja")]
        public string OznakaUpozorenja { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Cena mora biti broj veci ili jednak 0.")]
        [Display(Name = "Cena (RSD)")]
        public decimal Cena { get; set; }

        // dodeljeni tip vozila (rezultat poslovnog pravila - samo za prikaz)
        public string DodeljeniTipVozila { get; set; }

        // --- DETALJI: stavke paketa ---
        public List<StavkaUnosViewModel> Stavke { get; set; }

        public PaketUnosViewModel()
        {
            Stavke = new List<StavkaUnosViewModel>();
        }
    }
}

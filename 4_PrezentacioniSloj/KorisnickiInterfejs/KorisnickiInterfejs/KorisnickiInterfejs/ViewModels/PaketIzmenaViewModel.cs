using System.ComponentModel.DataAnnotations;

namespace KorisnickiInterfejs.ViewModels
{
    // ViewModel za izmenu paketa (MVC). Kod paketa je kljuc i ne menja se.
    public class PaketIzmenaViewModel
    {
        [Required]
        [Display(Name = "Kod paketa")]
        public string KodPaketa { get; set; }

        [Required(ErrorMessage = "Ime posiljaoca je obavezno.")]
        [Display(Name = "Posiljalac - ime")]
        public string PosiljalacIme { get; set; }

        [Required(ErrorMessage = "Adresa posiljaoca je obavezna.")]
        [Display(Name = "Posiljalac - adresa")]
        public string PosiljalacAdresa { get; set; }

        [Required(ErrorMessage = "Grad posiljaoca je obavezan.")]
        [Display(Name = "Posiljalac - grad")]
        public string PosiljalacGrad { get; set; }

        [Required(ErrorMessage = "Ime primaoca je obavezno.")]
        [Display(Name = "Primalac - ime")]
        public string PrimalacIme { get; set; }

        [Required(ErrorMessage = "Adresa primaoca je obavezna.")]
        [Display(Name = "Primalac - adresa")]
        public string PrimalacAdresa { get; set; }

        [Required(ErrorMessage = "Postanski broj je obavezan.")]
        [Display(Name = "Primalac - postanski broj")]
        public string PrimalacPostanskiBroj { get; set; }

        [Required(ErrorMessage = "Grad primaoca je obavezan.")]
        [Display(Name = "Primalac - grad")]
        public string PrimalacGrad { get; set; }

        [Range(0.01, 100000, ErrorMessage = "Tezina mora biti broj veci od 0.")]
        [Display(Name = "Tezina (kg)")]
        public decimal Tezina { get; set; }

        [Display(Name = "Oznaka upozorenja")]
        public string OznakaUpozorenja { get; set; }

        [Range(0.01, 10000000, ErrorMessage = "Cena mora biti broj veci od 0.")]
        [Display(Name = "Cena (RSD)")]
        public decimal Cena { get; set; }

        // samo za prikaz - trenutno/dodeljeno vozilo
        [Display(Name = "Dodeljeni tip vozila")]
        public string DodeljeniTipVozila { get; set; }
    }
}

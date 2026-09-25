using System.ComponentModel.DataAnnotations;

namespace KorisnickiInterfejs.ViewModels
{
    // ViewModel za prijavu korisnika (MVC).
    public class PrijavaViewModel
    {
        [Required(ErrorMessage = "Korisnicko ime je obavezno.")]
        [Display(Name = "Korisnicko ime")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Sifra je obavezna.")]
        [DataType(DataType.Password)]
        [Display(Name = "Sifra")]
        public string Sifra { get; set; }
    }
}

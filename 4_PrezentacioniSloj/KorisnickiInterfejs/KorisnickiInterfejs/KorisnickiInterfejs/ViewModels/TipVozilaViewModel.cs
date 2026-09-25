using System.ComponentModel.DataAnnotations;

namespace KorisnickiInterfejs.ViewModels
{
    // VIEW MODEL za sifarnik TipVozila (EF CRUD kroz MVC).
    public class TipVozilaViewModel
    {
        [Required(ErrorMessage = "Sifra je obavezna.")]
        [StringLength(10, ErrorMessage = "Sifra moze imati najvise 10 karaktera.")]
        [Display(Name = "Sifra")]
        public string Sifra { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan.")]
        [StringLength(50, ErrorMessage = "Naziv moze imati najvise 50 karaktera.")]
        [Display(Name = "Naziv")]
        public string Naziv { get; set; }
    }
}

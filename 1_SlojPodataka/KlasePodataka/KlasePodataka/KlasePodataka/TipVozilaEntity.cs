using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlasePodataka
{
    // ENTITY FRAMEWORK (EF6) ENTITET - mapira se na tabelu TipVozila.
    // Analogno referenci gde je EF koriscen za sifarnik "Tehnologije",
    // ovde se EF koristi za sifarnik "TipVozila".
    [Table("TipVozila")]
    public class TipVozilaEntity
    {
        [Key]
        [Column("Sifra")]
        [StringLength(10)]
        public string Sifra { get; set; }

        [Required]
        [Column("Naziv")]
        [StringLength(50)]
        public string Naziv { get; set; }
    }
}

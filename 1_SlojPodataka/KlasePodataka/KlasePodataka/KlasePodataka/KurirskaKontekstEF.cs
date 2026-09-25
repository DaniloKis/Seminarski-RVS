using System;
using System.Data.Entity;

namespace KlasePodataka
{
    // ENTITY FRAMEWORK 6 - DbContext (kontekst baze).
    // Konekcija se cita iz web.config-a po imenu "NasaKonekcija"
    // (ista konekcija koju koristi i ostatak aplikacije).
    public class KurirskaKontekstEF : DbContext
    {
        public KurirskaKontekstEF()
            : base("name=NasaKonekcija")
        {
            // EF ne sme da pokusava da kreira / migrira postojecu bazu
            Database.SetInitializer<KurirskaKontekstEF>(null);
        }

        public DbSet<TipVozilaEntity> TipoviVozila { get; set; }
    }
}

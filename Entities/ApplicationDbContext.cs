using Microsoft.EntityFrameworkCore;


namespace Entities
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            // Seed data for Countries
            string countriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach (Country c in countries)
            {
                modelBuilder.Entity<Country>().HasData(c);
            }

            // Seed data for Persons
            string personsJson = System.IO.File.ReadAllText("persons.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (Person p in persons)
            {
                modelBuilder.Entity<Person>().HasData(p);

            }
            //Fluent API configurations
            modelBuilder.Entity<Person>().Property(p => p.TIN)
                    .HasColumnName("TaxIdentificationNumber")
                    .HasColumnType("varchar(8)")
                    .HasDefaultValue("ABC12345");

            modelBuilder.Entity<Person>().ToTable(
                tb=>tb.HasCheckConstraint("CK_TIN,TIN must be 8 characters long", "LEN(TaxIdentificationNumber) = 8"));


            // Configure the relationship between Person and Country
            //--using data annotations in the Person class

            //modelBuilder.Entity<Person>()
            //    .HasOne(p => p.Country)
            //    .WithMany(c => c.Persons)
            //    .HasForeignKey(p => p.CountryID)
            //    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

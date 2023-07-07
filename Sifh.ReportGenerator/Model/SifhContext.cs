using System.Data.Entity;

namespace Sifh.ReportGenerator.Model
{
    public partial class SifhContext : DbContext
    {
        public SifhContext()
            : base("name=SifhMisDB")
        {
        }

        public virtual DbSet<Airline> Airlines { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<GradeClass> GradeClasses { get; set; }
        public virtual DbSet<NextOrderNumber> NextOrderNumbers { get; set; }
        public virtual DbSet<PackingList> PackingLists { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductStatusClass> ProductStatusClasses { get; set; }
        public virtual DbSet<ProductUnitePriceHistory> ProductUnitePriceHistories { get; set; }
        public virtual DbSet<ReceivingNote> ReceivingNotes { get; set; }
        public virtual DbSet<ReceivingNoteItem> ReceivingNoteItems { get; set; }
        public virtual DbSet<StatusClass> StatusClasses { get; set; }
        public virtual DbSet<Vessel> Vessels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airline>()
                .Property(e => e.AirlineName)
                .IsUnicode(false);

            modelBuilder.Entity<Airline>()
                .HasMany(e => e.PackingLists)
                .WithRequired(e => e.Airline)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.CustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.PackingLists)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GradeClass>()
                .Property(e => e.GradeClassName)
                .IsUnicode(false);

            modelBuilder.Entity<GradeClass>()
                .HasMany(e => e.ReceivingNoteItems)
                .WithRequired(e => e.GradeClass)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PackingList>()
                .Property(e => e.InvoiceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.CurrentUnitPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductUnitePriceHistories)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ReceivingNoteItems)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductStatusClass>()
                .Property(e => e.ProductStatusClassName)
                .IsUnicode(false);

            modelBuilder.Entity<ProductStatusClass>()
                .HasMany(e => e.ReceivingNoteItems)
                .WithRequired(e => e.ProductStatusClass)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductUnitePriceHistory>()
                .Property(e => e.UnitPriceOld)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ProductUnitePriceHistory>()
                .Property(e => e.UnitPriceNew)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ReceivingNote>()
                .Property(e => e.ReferenceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ReceivingNote>()
                .Property(e => e.TotalPayments)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ReceivingNote>()
                .Property(e => e.CheckNumber1)
                .IsUnicode(false);

            modelBuilder.Entity<ReceivingNote>()
                .Property(e => e.CheckNumber2)
                .IsUnicode(false);

            modelBuilder.Entity<ReceivingNoteItem>()
                .Property(e => e.UnitPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ReceivingNoteItem>()
                .Property(e => e.LineTotal)
                .HasPrecision(38, 6);

            modelBuilder.Entity<ReceivingNoteItem>()
                .Property(e => e.SpeciesCode)
                .IsUnicode(false);

            modelBuilder.Entity<StatusClass>()
                .Property(e => e.StatusClassName)
                .IsUnicode(false);

            modelBuilder.Entity<StatusClass>()
                .HasMany(e => e.ReceivingNotes)
                .WithRequired(e => e.StatusClass)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.VesselName)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.ContactName)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.ContactNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.RegistrationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .HasMany(e => e.ReceivingNotes)
                .WithRequired(e => e.Vessel)
                .WillCascadeOnDelete(false);
        }
    }
}

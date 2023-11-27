using Microsoft.EntityFrameworkCore;
using Vila.Data.Entities;

namespace Vila.Data
{
    public class VilaDbContext : DbContext
    {
        public VilaDbContext(DbContextOptions<VilaDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UsersEO>()
                        .HasMany(u => u.VillaDetails)
                        .WithOne(v => v.OwnerUser)
                        .HasForeignKey(v => v.UserID);


            modelBuilder.Entity<VillaDetailsEO>()
                        .HasOne(v => v.vilaDescription)
                        .WithOne(d => d.villaDetails)
                        .HasForeignKey<VillaDetailsEO>(v => v.VilaId);

            modelBuilder.Entity<VillaDetailsEO>()
                       .HasOne(v => v.CheckListVila)
                       .WithOne(d => d.VillaDetails)
                       .HasForeignKey<VillaDetailsEO>(v => v.VilaId);

            modelBuilder.Entity<VillaDetailsEO>()
                      .HasMany(v => v.Photos)
                      .WithOne(d => d.VillaDetails)
                      .HasForeignKey(v => v.VilaId);


            /*
            * ואני יוצר את הטבלה בדאטה בייס ישירות מפה add-migration יש צורך בזה אם אני עושה את הפקודה בכומנד 
            * update-database הפקודה השניה כדי לעדכן 
            modelBuilder.Entity<UsersEO>().ToTable("users");
            */
            // modelBuilder.Entity<UsersEO>().HasKey(ui => new { ui.UserID });
        }

        public virtual DbSet<UsersEO> Users { get; set; }
       // public virtual DbSet<VilaBathroomEO> Bathroom { get; set; }
       // public virtual DbSet<VilaBedroomLaundryEO> BedroomLaundry { get; set; }
      //  public virtual DbSet<VilaFamilyEO> Family { get; set; }
        public virtual DbSet<CheckListVilaEO> checkListVila { get; set; }
        public virtual DbSet<VilaPhoto> Photos { get; set; }
        public virtual DbSet<VillaDescriptionEO> Description { get; set; }
        public virtual DbSet<VillaDetailsEO> Details { get; set; }





        //for tasts
        public virtual DbSet<Test> t { get; set; }
    }
}

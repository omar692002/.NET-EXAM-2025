using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GestionParfumerie.Domain.Entities;

namespace GestionParfumerie.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // --- Tables de ton domaine ---
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }
        public DbSet<FamilleOlfactive> FamillesOlfactives { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<LigneCommande> LigneCommandes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relations Produit -> Categorie
            builder.Entity<Produit>()
                .HasOne(p => p.Categorie)
                .WithMany(c => c.Produits)
                .HasForeignKey(p => p.CategorieId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relations Produit -> Fournisseur
            builder.Entity<Produit>()
                .HasOne(p => p.Fournisseur)
                .WithMany(f => f.Produits)
                .HasForeignKey(p => p.FournisseurId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relations Produit -> FamilleOlfactive
            builder.Entity<Produit>()
                .HasOne(p => p.FamilleOlfactive)
                .WithMany(f => f.Produits)
                .HasForeignKey(p => p.FamilleOlfactiveId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relations Commande -> ApplicationUser (au lieu de Client)
            builder.Entity<Commande>()
                .HasOne(c => c.Utilisateur)
                .WithMany(u => u.Commandes)
                .HasForeignKey(c => c.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relations LigneCommande -> Commande et Produit
            builder.Entity<LigneCommande>()
                .HasOne(lc => lc.Commande)
                .WithMany(c => c.LigneCommandes)
                .HasForeignKey(lc => lc.CommandeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LigneCommande>()
                .HasOne(lc => lc.Produit)
                .WithMany(p => p.LigneCommandes)
                .HasForeignKey(lc => lc.ProduitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Commande>()
                .Property(c => c.Total)
                .HasPrecision(18, 4); 

            builder.Entity<LigneCommande>()
                .Property(lc => lc.PrixUnitaire)
                .HasPrecision(18, 4);

        }
    }
}

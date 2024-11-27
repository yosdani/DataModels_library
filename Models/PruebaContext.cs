using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace Datamodels.Models
{
   
    public partial class PruebaContext :DbContext
    {
        
        
     
        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<GeneralStatus> GeneralStatuses { get; set; }


        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Role", "Prueba.dbo", tb => tb.HasComment("Tabela que contém os perfis de utilizador."));
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("now()");
                entity.Property(e => e.UpdatedDate).HasDefaultValueSql("now()");
            });
            modelBuilder.Entity<GeneralStatus>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("general_status_pkey");
                entity.Property(e => e.Id).HasDefaultValueSql("nextval('seamind_data.user_status_id_seq'::regclass)");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("now()");
                entity.Property(e => e.UpdatedDate).HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("user_pkey");
                entity.ToTable("User", "Prueba.dbo", tb => tb.HasComment("contiene los usuarios registrados."));
                object value = entity.Property(e => e.CreatedDate).HasDefaultValueSql("now()");
                entity.Property(e => e.UpdatedDate).HasDefaultValueSql("now()");
                entity.HasOne(d => d.Role).WithMany(p => p.Users).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_user_role_id");
                entity.HasOne(d => d.Status).WithMany(p => p.Users).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_user_status_id");
            });



















            modelBuilder.HasSequence("user_id_seq", "seamind_data");
            modelBuilder.HasSequence("user_role_id_seq", "seamind_data");
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace Datamodels.Models
{

    public partial class PruebaContext : DbContext
    {



        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<GeneralStatus> GeneralStatuses { get; set; }
        public virtual DbSet<Student> Students { get; set; }


        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {

                b.Property<string>("Password")
                 .HasColumnType("nvarchar(max)");

                b.Property<string>("Email")
                    
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                
                   .HasColumnType("nvarchar(max)");
                b.Property<string>("Surname")
                  .HasColumnType("nvarchar(max)");

                b.Property<string>("Token")
                   .HasColumnType("nvarchar(max)");
               
                b.Property<int>("RoleId")
                      .HasColumnType("int");
                b.Property<int>("StatusId")
                      .HasColumnType("int");
                b.HasKey("Id");
                b.HasIndex("RoleId");
                b.HasIndex("StatusId");

                b.ToTable("User");
            });
            modelBuilder.Entity<User>()


       .Navigation(b => b.Role)
       .UsePropertyAccessMode(PropertyAccessMode.Property);
            modelBuilder.Entity<User>()


      .Navigation(b => b.Status)
      .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<Role>()
            .Navigation(b => b.Users)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<GeneralStatus>()
           .Navigation(b => b.Users)
           .UsePropertyAccessMode(PropertyAccessMode.Property);


            modelBuilder.Entity<Role>(b =>
            {



                b.Property<string>("Name_es")
                    
                    .HasColumnType("nvarchar(max)");



                b.Property<string>("Name_en")
                   .HasColumnType("nvarchar(max)");
                b.Property<string>("Name_en")
                  .HasColumnType("nvarchar(max)");
                b.Property<string>("Description_en")
                  .HasColumnType("nvarchar(max)");
                b.Property<string>("Description_es")
                  .HasColumnType("nvarchar(max)");


                b.ToTable("Role");
            });
            modelBuilder.Entity<Student>(b =>
            {



                b.Property<string>("Name")
                   
                    .HasColumnType("nvarchar(max)");



                b.Property<string>("Sex")
                   .HasColumnType("nvarchar(max)");

                b.Property<int>("Age")
                      .HasColumnType("int");
                b.Property<string>("Specialty")
                    .HasColumnType("nvarchar(max)");
                b.Property<int>("Year")
                      .HasColumnType("int");

                b.ToTable("Student");
            });


            modelBuilder.Entity<GeneralStatus>(b =>
            {



                b.Property<string>("Name_en")
                   
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name_es")
                   
                   .HasColumnType("nvarchar(max)");

              

                b.ToTable("GeneralStatus");
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Datamodels.Models
{
    /// <summary>
    /// Tabela que contém os dados dos utilizadores registados.
    /// </summary>
    [Table("user", Schema = "Prueba")]
    [Index("Email", Name = "user_email_key", IsUnique = true)]
    public partial class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [Column("surname")]
        [StringLength(64)]
        public string Surname { get; set; }

        [Required]
        [Column("email")]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [Column("password", TypeName = "character varying")]
        public string Password { get; set; }


        [Column("created_date", TypeName = "timestamp without time zone")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_date", TypeName = "timestamp without time zone")]
        public DateTime UpdatedDate { get; set; }

        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("token", TypeName = "character varying")]
        public string Token { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("Users")]
        public virtual Role Role { get; set; }

        [ForeignKey("StatusId")]
        [InverseProperty("Users")]
       
        public virtual GeneralStatus Status { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datamodels.Models
{
    [Table("general_status", Schema = "prueba")]
    [Index("NameEn", Name = "uk_general_status_name_en", IsUnique = true)]
    [Index("NameEs", Name = "uk_general_status_name_es", IsUnique = true)]
    public partial class GeneralStatus
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name_en")]
        [StringLength(15)]
        public string NameEn { get; set; }

        [Required]
        [Column("name_es")]
        [StringLength(15)]
        public string NameEs { get; set; }

        [Column("created_date")]
        public TimeOnly CreatedDate { get; set; }

        [Column("updated_date")]
        public TimeOnly UpdatedDate { get; set; }


        [InverseProperty("Status")]
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}

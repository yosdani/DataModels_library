using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;

namespace Datamodels.Models
{
    [Table("role", Schema = "Prueba.dbo")]
    
    public partial class Role
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name_es")]
        [StringLength(100)]
        public string NameEs { get; set; }

        [Required]
        [Column("name_en")]
        [StringLength(100)]
        public string NameEn { get; set; }

        [Column("description_es")]
        [StringLength(100)]
        public string DescriptionEs { get; set; }

        [Column("description_en")]
        [StringLength(100)]
        public string DescriptionEn { get; set; }

        [Column("created_date", TypeName = "timestamp without time zone")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_date", TypeName = "timestamp without time zone")]
        public DateTime UpdatedDate { get; set; }


        [InverseProperty("Role")]
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}

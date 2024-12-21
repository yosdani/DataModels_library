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
    [Table("Prueba.dbo.general_status", Schema = "Prueba.dbo")]
  
    public partial class GeneralStatus
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        
        [Column("name_en")]
        [StringLength(15)]
        public string? NameEn { get; set; }

        
        [Column("name_es")]
        [StringLength(15)]
        public string? NameEs { get; set; }

        [Column("created_date")]
        public TimeOnly CreatedDate { get; set; }

        [Column("updated_date")]
        public TimeOnly UpdatedDate { get; set; }


        [InverseProperty("Status")]
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}

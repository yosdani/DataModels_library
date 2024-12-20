using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datamodels.Models
{

    [Table("Prueba.dbo.student", Schema = "Prueba.dbo")]
    public partial class Student
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [Column("sex", TypeName = "character varying")]
        public string Sex { get; set; }


        [Required]
        [Column("age", TypeName = "integer")]
        public int Age { get; set; }

        [Column("specialty", TypeName = "character varying")]
        public string Specialty { get; set; }
       
        [Column("year", TypeName = "integer")]
        public int Year { get; set; }

     

        

    }
}

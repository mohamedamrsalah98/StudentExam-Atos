using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSytemData.Dto
{
    public class SubjectDto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "The SubjectName field is required.")]
        public string SubjectName { get; set; }
    }
}

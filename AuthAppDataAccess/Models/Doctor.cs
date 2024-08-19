using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDataAccess.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }  
        public string? UniversityDegree { get; set; }
        public string ? Address { get; set; }   

    }
}

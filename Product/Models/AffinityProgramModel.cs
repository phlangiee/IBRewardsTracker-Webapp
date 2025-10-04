using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace AffinityProgram_Namespace.Models
{
    public class AffinityProgram
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int TypeID { get; set; }

        [Required]
        public string ProgramCompany { get; set; }

        [Required]
        public string AffinityNum { get; set; }

        [Required]
        public string? Level { get; set; }

    }
}
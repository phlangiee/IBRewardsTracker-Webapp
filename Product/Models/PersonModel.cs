using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Person_Namespace.Models
{
    public class Person
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

    }
    
}

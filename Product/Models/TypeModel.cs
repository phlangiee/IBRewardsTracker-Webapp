using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Type_Namespace.Models
{
    public class RewardType
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required] 
        public string Description { get; set; }

    }
}
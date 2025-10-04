using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using AffinityProgram_Namespace.Models;
using AffinityProgram_Namespace.Data;

namespace AffinityCard_Namespace.Models
{
    public class AffinityCard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AffinityProgramID { get; set; }

        [Required]
        public int PersonID { get; set; }

        [Required]
        public string RewardCompany { get; set; }

        [Required]
        public int Points { get; set; }

        [Required]
        public string DateOpen { get; set; }

        public string? DateClose { get; set; }

        public int? AnnualFee { get; set; }

        public int? CreditLine { get; set; }

        public string? Notes { get; set; }

        public string getAffinityProgramName(IEnumerable<AffinityProgram> programs)
        {
            var list = programs.ToList();
            
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Id == this.AffinityProgramID)
                {
                    return list[i].ProgramCompany;
                }
            }

            return "";
        }
    }

    public class IdPoints
    {

        public int Id { get; set; }

        public int Points { get; set; }

    }
}

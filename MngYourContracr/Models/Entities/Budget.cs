
namespace MngYourContracr.MngYourContractDatabase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Budget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string BudgetId { get; set; }

        public string ProjectId { get; set; }

        public decimal Income { get; set; }

        public decimal Outgoings { get; set; }

    }
}
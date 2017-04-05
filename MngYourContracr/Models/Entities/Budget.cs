
namespace MngYourContracr.MngYourContractDatabase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Budget
    {
        [Key]
        public string BudgetId { get; set; }

        public string ProjectId { get; set; }

        public decimal Income { get; set; }

        public decimal Outgoings { get; set; }

    }
}
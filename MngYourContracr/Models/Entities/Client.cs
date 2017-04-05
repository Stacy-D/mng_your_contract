

namespace MngYourContracr.MngYourContractDatabase
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using MngYourContracr.Models;

    /*
     * Represents user of the service 
     */
    public class Client
    {
        [Key]
        public string ClientId { get; set; }

        [ForeignKey("ClientId")]
        public ApplicationUser User { get; set; }

        /*
         Gets or sets the list of client's contracts
       */
        public List<Project> Projects { get; set; }
    }
}
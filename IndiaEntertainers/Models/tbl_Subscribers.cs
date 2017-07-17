namespace IndiaEntertainers.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Subscribers
    {
        [Key]
        public long SID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string EmailId { get; set; }

        [Display(Name = "Subscribe Date")]
        public DateTime? Subscribedate { get; set; }

        [Display(Name = "Is Active?")]
        public bool? IsActive { get; set; }

        [StringLength(50)]
        [Display(Name = "IP Address")]
        public string IPAddress { get; set; }
    }
}

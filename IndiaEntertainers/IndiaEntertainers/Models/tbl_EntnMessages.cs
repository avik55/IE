namespace IndiaEntertainers.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_EntnMessages
    {
        [Key]
        public long EMID { get; set; }

        [StringLength(50)]
        [Display(Name = "IP Address")]
        public string IPAddress { get; set; }

        [Display(Name = "Entertainer")]
        public long? EntrId { get; set; }

        [StringLength(128)]
        [Display(Name = "Sender")]
        public string SenderUserId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Contact No.")]
        public string ContactNo { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "EmailId")]
        public string EmailID { get; set; }

        [Required]
        [StringLength(150)]
        public string Message { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [Display(Name = "Is Read?")]
        public bool? IsRead { get; set; }

        [Display(Name = "Received DateTime")]
        public DateTime? ReceivedDateTime { get; set; }

        public virtual tbl_Entertainer tbl_Entertainer { get; set; }
    }
}

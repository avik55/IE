namespace IndiaEntertainers.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_TalentSeeker
    {
        [Key]
        public long TlSkId { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(350)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Comp. Email")]
        public string CompEmail { get; set; }

        [StringLength(50)]
        [Display(Name = "Comp. Mobile")]
        public string ComMobile { get; set; }

        [StringLength(250)]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [StringLength(50)]
        [Display(Name = "CP Contact No.")]
        public string CPContNo { get; set; }

        [StringLength(100)]
        [Display(Name = "CP Email")]
        public string CPEmail { get; set; }

        [StringLength(50)]
        [Display(Name = "Office No.")]
        public string OfficeNo { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [StringLength(250)]
        [Display(Name = "Middle Name")]
        public string MName { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [StringLength(50)]
        public string Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Mobile { get; set; }

        [Display(Name = "State")]
        public int? StateId { get; set; }

        [Display(Name = "City")]
        public int? CityId { get; set; }

        [StringLength(550)]
        public string Address { get; set; }

        [StringLength(50)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Reg. Date")]
        public DateTime? RegDate { get; set; }

        [Display(Name = "Is Active?")]
        public bool? IsActive { get; set; }

        [Display(Name = "Is Premium?")]
        public bool? IsPremium { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }
    }
}

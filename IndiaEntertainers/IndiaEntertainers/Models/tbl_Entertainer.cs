namespace IndiaEntertainers.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Entertainer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Entertainer()
        {
            tbl_EntertainerCats = new HashSet<tbl_EntertainerCats>();
            tbl_EntnMessages = new HashSet<tbl_EntnMessages>();
            tbl_EntrImages = new HashSet<tbl_EntrImages>();
            tbl_EntrVideos = new HashSet<tbl_EntrVideos>();
            tbl_Projectography = new HashSet<tbl_Projectography>();
        }

        [Key]
        [Display(Name = "Entertainer Id")]
        public long EntrId { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [StringLength(250)]
        [Display(Name = "Middle Name")]
        public string MName { get; set; }

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

        [StringLength(250)]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [StringLength(50)]
        [Display(Name = "CP Contact No.")]
        public string CPContNo { get; set; }

        [StringLength(100)]
        public string CPEmail { get; set; }

        [StringLength(50)]
        [Display(Name = "Office No.")]
        public string OfficeNo { get; set; }

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

        [StringLength(100)]
        public string Type { get; set; }

        [Display(Name = "No. of Members")]
        public int? Members { get; set; }

        public int? Star { get; set; }

        public string Profile { get; set; }

        [Display(Name = "Profile Title")]
        public string ProfileTitle { get; set; }

        [StringLength(150)]
        public string Nationality { get; set; }

        [Display(Name = "No. of Show")]
        public int? NoofShow { get; set; }

        [StringLength(250)]
        [Display(Name = "Performance Language")]
        public string PerfLanguage { get; set; }

        [StringLength(250)]
        [Display(Name = "PERFORMING MEMBERS")]
        public string PERFORMINGMEMBERS { get; set; }

        [Display(Name = "FB PageLink")]
        public string FBPageLink { get; set; }

        [Display(Name = "Twiter PageLink")]
        public string TwiterPageLink { get; set; }

        [Display(Name = "Intgram PageLink")]
        public string IntgramPageLink { get; set; }


        [Display(Name = "Performance fees")]
        public string Performancefee { get; set; }

        [Display(Name = "Show fee?")]
        public bool Showfee { get; set; }

        [Display(Name = "Performance LENGTH")]
        public int PerfLength { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EntertainerCats> tbl_EntertainerCats { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EntnMessages> tbl_EntnMessages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EntrImages> tbl_EntrImages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EntrVideos> tbl_EntrVideos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Projectography> tbl_Projectography { get; set; }
    }
}

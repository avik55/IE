namespace IndiaEntertainers.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Projectography
    {
        [Key]
        public int PrGId { get; set; }

        [Display(Name = "Entertainer")]
        public long EntrId { get; set; }

        [Required]
        [StringLength(350)]
        public string Title { get; set; }

        [StringLength(350)]
        public string Time { get; set; }

        [StringLength(650)]
        public string Venue { get; set; }

        [Display(Name = "State")]
        public int? StateId { get; set; }

        [Display(Name = "City")]
        public int? CityId { get; set; }

        public string Description { get; set; }

        [StringLength(350)]
        public string Remark { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UploadDate { get; set; }

        public virtual tbl_Entertainer tbl_Entertainer { get; set; }
    }
}

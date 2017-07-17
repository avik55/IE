namespace IndiaEntertainers.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_EntrImages
    {
        [Key]
        public long ImageId { get; set; }

        [Display(Name = "Entertainer")]
        public long EntrId { get; set; }

        [StringLength(250)]
        public string Title { get; set; }

        [StringLength(350)]
        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public int? Orderby { get; set; }

        [StringLength(250)]
        public string Type { get; set; }

        [Display(Name = "Is Current Profile Image")]
        public bool? IsCurProfileImg { get; set; }

        [Display(Name = "Is Current Cover Image?")]
        public bool? IsCurCoverImg { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UploadDate { get; set; }

        public virtual tbl_Entertainer tbl_Entertainer { get; set; }
    }
}

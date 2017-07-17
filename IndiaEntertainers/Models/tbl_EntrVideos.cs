namespace IndiaEntertainers.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_EntrVideos
    {
        [Key]
        public long Vid { get; set; }

        [Display(Name = "Entertainer")]
        public long EntrId { get; set; }

        [StringLength(250)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Video Link")]
        public string Videolink { get; set; }

        [StringLength(350)]
        public string Name { get; set; }

        public int? Orderby { get; set; }

        [StringLength(350)]
        [Display(Name = "Cover Image")]
        public string CoverImage { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UploadDate { get; set; }

        public virtual tbl_Entertainer tbl_Entertainer { get; set; }
    }
}

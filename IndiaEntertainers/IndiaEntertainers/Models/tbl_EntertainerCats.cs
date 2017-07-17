namespace IndiaEntertainers.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_EntertainerCats
    {
        [Key]
        public long ECId { get; set; }


        [Display(Name = "Category")]
        public int? CatId { get; set; }

        [Display(Name = "Subcategory")]
        public int? SubCatId { get; set; }

        [Display(Name = "Entertainer")]
        public long EntrId { get; set; }

        public virtual tbl_Category tbl_Category { get; set; }

        public virtual tbl_Entertainer tbl_Entertainer { get; set; }

        public virtual tbl_SubCategory tbl_SubCategory { get; set; }
    }
}

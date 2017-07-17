namespace IndiaEntertainers.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_SubCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_SubCategory()
        {
            tbl_EntertainerCats = new HashSet<tbl_EntertainerCats>();
        }

        [Key]
        public int SubCatId { get; set; }

        [Display(Name = "Category")]
        public int CatId { get; set; }

        [Display(Name = "Subcategory")]
        public int SubParentCatId { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Title")]
        public string SubTitle { get; set; }

        [StringLength(550)]
        [Display(Name = "Description")]
        public string SubDescription { get; set; }

        public int? OrderBy { get; set; }

        public virtual tbl_Category tbl_Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EntertainerCats> tbl_EntertainerCats { get; set; }
    }
}

namespace IndiaEntertainers.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class tbl_Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Category()
        {
            tbl_EntertainerCats = new HashSet<tbl_EntertainerCats>();
            tbl_SubCategory = new HashSet<tbl_SubCategory>();
        }

        [Key]
        public int CatId { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [StringLength(550)]
        public string Description { get; set; }

        public bool? IsActive { get; set; }

        public int OrderBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EntertainerCats> tbl_EntertainerCats { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SubCategory> tbl_SubCategory { get; set; }
    }
}

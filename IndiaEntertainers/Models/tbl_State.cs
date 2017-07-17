namespace IndiaEntertainers.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_State
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_State()
        {
            tbl_City = new HashSet<tbl_City>();
        }

        [Key]
        public int StateId { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Stat Name")]
        public string StateName { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_City> tbl_City { get; set; }
    }
}

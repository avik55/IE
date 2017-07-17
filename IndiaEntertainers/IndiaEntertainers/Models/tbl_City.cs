namespace IndiaEntertainers.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_City
    {
        [Key]
        public int CityId { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "City Name")]
        public string CityName { get; set; }

        [Display(Name = "State")]
        public int? StateId { get; set; }

        public virtual tbl_State tbl_State { get; set; }
    }
}

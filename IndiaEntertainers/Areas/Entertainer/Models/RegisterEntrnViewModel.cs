using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IndiaEntertainers.Areas.Entertainer.Models
{
    public class RegisterEntrnViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Artist/Group Name")]
        public string FName { get; set; }

        [Required]
        [StringLength(50)]
        [Phone]
        public string Mobile { get; set; }

        [Required]
        public int StateId { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public int CatId { get; set; }

        [Display(Name = "Performance fees")]
        public string Performancefee { get; set; }

        [StringLength(100)]
        public string Type { get; set; }

        [StringLength(150)]
        public string Nationality { get; set; }

        [Display(Name = "Performance Length")]
        public int PerfLength { get; set; }

        [StringLength(50)]
        public string Gender { get; set; }

        [StringLength(250)]
        [Display(Name = "Number of years you have been performing")]
        public string YearsOfPerforming { get; set; }

        [StringLength(250)]
        [Display(Name = "Performance Language")]
        public string PerfLanguage { get; set; }

        public string Profile { get; set; }

        [Display(Name = "Profile Title")]
        public string ProfileTitle { get; set; }

        [Display(Name = "FB PageLink")]
        public string FBPageLink { get; set; }

        [Display(Name = "Twiter PageLink")]
        public string TwiterPageLink { get; set; }

        [Display(Name = "Intgram PageLink")]
        public string IntgramPageLink { get; set; }

    }
}
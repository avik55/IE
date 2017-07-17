using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IndiaEntertainers.ViewModel
{
    public class RegisterViewModel
    {
        [Key]
        [Display(Name = "Entertainer Id")]
        public long EntrId { get; set; }

        [Required(ErrorMessage = "Please enter Artist/Group Name")]
        [StringLength(250)]
        [Display(Name = "Artist/Group Name")]
        public string FName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]        
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered Mobile format is not valid.")]
        public string Mobile { get; set; }

        [Required]
        [Display(Name = "State")]
        public int? StateId { get; set; }

        [Required]
        [Display(Name = "City")]
        public int? CityId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int? CatId { get; set; }

        [Required]
        [Display(Name = "Category Pricing (in Rs.)")]
        public decimal Performancefee { get; set; }

        //[Required]
        [Display(Name = "Number of Performing Members")]
        public int? Members { get; set; }

        [Required]
        [Display(Name = "Performance length (in minutes)")]
        public int PerfLength { get; set; }

      //[Required]
        [StringLength(250)]
        [Display(Name = "Number of years you have been performing")]
        public string YearsOfPerforming { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Performance Language")]
        public string PerfLanguage { get; set; }

        //[Required]
        //[StringLength(150)]
        //public string Nationality { get; set; }

        [Required]
        [Display(Name = "Tagline (Define your talent / act in the most attractive one-liner)")]
        public string ProfileTitle { get; set; }

        [Required]
        public string Profile { get; set; }

        [Required]
        public HttpPostedFileBase profileimg { get; set; }

        [Required]
        public HttpPostedFileBase coverimg { get; set; }

        [Required]
        [StringLength(50)]
        public string Gender { get; set; }

        [Display(Name = "FB PageLink")]
        public string FBPageLink { get; set; }

        [Display(Name = "Twiter PageLink")]
        public string TwiterPageLink { get; set; }

        [Display(Name = "Intgram PageLink")]
        public string IntgramPageLink { get; set; }

        public bool IsItIndian { get; set; }

        public string UserId { get; set; }

        [StringLength(100)]
        public string Type { get; set; }

        public string PerfMembers { get; set; }

        [Display(Name = "No. of Show")]
        public int? NoofShow { get; set; }

        [StringLength(250)]
        [Display(Name = "PERFORMING MEMBERS")]
        public string PERFORMINGMEMBERS { get; set; }
    }
}
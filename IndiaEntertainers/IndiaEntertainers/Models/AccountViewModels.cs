using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IndiaEntertainers.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
       // [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string UserType { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class EntRegisterViewModel
    {
        [Required]
        [StringLength(250)]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [StringLength(250)]
        [Display(Name = "Middle Name")]
        public string MName { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [StringLength(50)]
        public string Gender { get; set; }

        public DateTime? DOB { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Mobile { get; set; }

        [StringLength(250)]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [StringLength(50)]
        [Display(Name = "CPContact No.")]
        public string CPContNo { get; set; }

        [StringLength(100)]
        [Display(Name = "CP Email")]
        public string CPEmail { get; set; }

        [StringLength(50)]
        [Display(Name = "Office No.")]
        public string OfficeNo { get; set; }

        [Display(Name = "State")]
        public int? StateId { get; set; }

        [Display(Name = "City")]
        public int? CityId { get; set; }

        [StringLength(550)]
        public string Address { get; set; }

        [StringLength(50)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [StringLength(100)]
        public string Type { get; set; }

        public int? Members { get; set; }

        public int? Star { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class TSRegisterViewModel
    {

        [Required]
        [StringLength(250)]
        public string FName { get; set; }

        [StringLength(250)]
        public string MName { get; set; }

        [Required]
        [StringLength(250)]
        public string LName { get; set; }

        [StringLength(350)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(100)]
        public string CompEmail { get; set; }

        [StringLength(50)]
        public string ComMobile { get; set; }

        [StringLength(250)]
        public string ContactPerson { get; set; }

        [StringLength(50)]
        public string CPContNo { get; set; }

        [StringLength(100)]
        public string CPEmail { get; set; }

        [StringLength(50)]
        public string OfficeNo { get; set; }
       
        [StringLength(50)]
        public string Gender { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Mobile { get; set; }

        public int? StateId { get; set; }

        public int? CityId { get; set; }

        [StringLength(550)]
        public string Address { get; set; }

        [StringLength(50)]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}

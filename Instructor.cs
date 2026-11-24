using System.ComponentModel.DataAnnotations;

namespace InstructorInformationManagement.Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Instructor ID is required")]
        [Display(Name = "Instructor ID")]
        public string InstructorIdNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Addresas is required")]
        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Age is required")]
        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
        [Display(Name = "Age")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Majorship is required")]
        [Display(Name = "Majorship")]
        public string Majorship { get; set; } = string.Empty;

        [Required(ErrorMessage = "Designation is required")]
        [Display(Name = "Designation")]
        public string Designation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bachelor Degree is required")]
        [Display(Name = "Bachelor Degree")]
        public string BachelorDegree { get; set; } = string.Empty;

        // Tracks which user account created this record for data isolation
        public string? UserEmail { get; set; }
    }
}
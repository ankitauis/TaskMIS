using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMIS.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Email")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StrongPassword(ErrorMessage = "Password must be at least 8 characters long and include a combination of upper case, lower case, digits, and special characters.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [NotMapped] 
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public virtual ICollection<TaskEntity> Tasks { get; set; } = new HashSet<TaskEntity>();
    }

    public class StrongPasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var password = value as string;
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            bool isLongEnough = password.Length >= 8;
            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = Regex.IsMatch(password, @"[\!@#\$%\^&\*\(\)_\+\-=\[\]\{\};':""\|,.<>\?\\/`~]");

            return isLongEnough && hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
        }
    }
}

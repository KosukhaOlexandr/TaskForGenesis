using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
namespace TaskForGenesis.BiewModel
{
    public class RegisterViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

    }
}

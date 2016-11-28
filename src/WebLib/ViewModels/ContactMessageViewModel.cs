using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLib.ViewModels
{
    public class ContactMessageViewModel
    {
        
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress]
        public string Email { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "The Message field is required.")]
        public string Message { get; set; }
    }
}

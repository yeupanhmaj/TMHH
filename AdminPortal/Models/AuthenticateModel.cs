using System.ComponentModel.DataAnnotations;
namespace AdminPortal.Models
{
        public class AuthenticateModel
        {
            [Required]
            public string userName { get; set; }

            [Required]
            public string password { get; set; }
        }
}

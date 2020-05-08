using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models
{
    public class SecurityQuestion
    {
        public int Id { get; set; }

        [Required]
        public String Question { get; set; }
    }
}

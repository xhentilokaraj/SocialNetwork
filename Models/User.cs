﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string UserType { get; set; } = "user";

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }



        [Required]
        [Display(Name = "Security Question Answer")]
        public string SecurityQuestionAnswer { get; set; }

        [Display(Name = "Security Question")]
        public int SecurityQuestionID { get; set; }

        [Display(Name = "Security Question")]
        public SecurityQuestion SecurityQuestion { get; set; }

    }
}
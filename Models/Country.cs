using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models
{
    public class Country
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        public ICollection<City> Cities { get; set; }


    }
}

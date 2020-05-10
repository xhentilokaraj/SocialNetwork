using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "City Name")]
        public string CityName { get; set; }

        [Display(Name = "Country")]
        public int CountryID { get; set; }


        [Display(Name = "Country")]
        public Country Country { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models
{
    public class Community
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Community Name")]
        public String CommunityName { get; set; }

        [Display(Name = "Owner")]
        public int OwnerId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        public String Status { get; set; } = "Inactive";

        public User Owner { get; set; }

        public ICollection<CommunityMember> CommunityMembers { get; set; }
    }
}

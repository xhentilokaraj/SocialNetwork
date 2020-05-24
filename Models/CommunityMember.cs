using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models
{
    public class CommunityMember
    {
        public int Id { get; set; }

        [Display(Name = "User")]
        public int UserId { get; set; }

        [Display(Name = "Community")]
        public int CommunityId { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Date of Joining")]
        public DateTime DateOfJoining { get; set; }

        public String Status { get; set; } = "Pending";

        public User User { get; set; }

        public Community Community { get; set; }

        
    }
}

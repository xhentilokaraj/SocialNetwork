using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models
{

    public class UserRelationship
    {
        public int Id { get; set; }

        public int RelatingUserId { get; set; }

        public int RelatedUserId { get; set; }

        public string RequestStatus { get; set; } = "Pending";

        public string RelationshipStatus { get; set; } = "Inactive";

        [DataType(DataType.Date)]
        public DateTime DateOfRequest { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfAcceptance { get; set; }

        public User RelatingUser { get; set; }

        public User RelatedUser { get; set; }
    }
}

using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rehber.Models
{
    public class Contact
    {
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "nvarchar(25)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(25)")]
        public string LastName { get; set; }
        [Column(TypeName = "nvarchar(11)")]
        public string PhoneNumber { get; set; }
        
        public bool IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

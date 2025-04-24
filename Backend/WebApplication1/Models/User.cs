using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryBackend.Models
{
    public class User
    {
        [Key]  // 👈 This is what EF needs to recognize the primary key
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Role { get; set; }
    }
}

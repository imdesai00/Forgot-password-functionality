
using System.ComponentModel.DataAnnotations;


namespace verfiyemail.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public Int64 PhoneNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


    }
}

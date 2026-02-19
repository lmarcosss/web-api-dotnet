using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Domain.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        public int id { get; private set; }
        public string name { get; private set; }
        public DateTime dateOfBirth { get; private set; }
        public string? photo { get; private set; }
        public string email { get; private set; }
        public string password { get; private set; }

        public User() { }

        public User(string name, DateTime dateOfBirth, string? photo, string email, string password)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.dateOfBirth = dateOfBirth;
            this.photo = photo;
            this.email = email ?? throw new ArgumentNullException(nameof(email));
            this.password = password ?? throw new ArgumentNullException(nameof(password));
        }
    }
}

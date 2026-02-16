using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Domain.Models
{
    [Table("company")]
    public class Company
    {
        [Key]

        public int id { get; private set; }
        public string name { get; private set; }

        public Company(string name)
        {
            this.name = name;
        }
    }
}
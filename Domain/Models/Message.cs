using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Context {  get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}

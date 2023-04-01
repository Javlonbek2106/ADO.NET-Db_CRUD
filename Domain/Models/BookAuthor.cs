using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class BookAuthor
    {
        public int BookAuthorId { get; set; }
        public Book Book { get; set; }
        public Author Author { get; set; }
    }
}

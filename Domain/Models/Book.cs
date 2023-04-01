using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PageCount { get; set; }

        public override string ToString()
        {
            return $"Bookid = {BookId},  Book Name = {BookName}, created date = {CreatedDate}, page count {PageCount}";
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRecommender.Entities
{
    public class BookDetails
    {
        public List<Book> TheBooks { get; set; } = new List<Book>();
        public List<BookUserRating> Ratings { get; set; } = new List<BookUserRating>();
        public List<User> Users { get; set; } = new List<User>();
    }
    public class Book
    {
        public string BookTitle { get; set;}
        public string BookAuthor { get; set;}   
        public string ISBN { get; set;}
        public string Publisher { get; set;}    
        public string YearOfPublication { get; set;}    
        public string ImageUrlSmall { get; set;}
        public string ImageUrlMedium { get; set;}
        public string ImageUrlLarge { get; set;}
        public List<BookUserRating> Ratings { get; set; } = new List<BookUserRating>();
    }
    public class BookUserRating
    {
        public string Rating { get; set;}   
        public string ISBN { get; set; }
        public string UserID { get; set;}   
        public Book TheBook { get; set;}
        public User TheUser { get; set;}    
    }
    public class User
    {
        public string UserID { get; set; }
        public int? Age { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public List<BookUserRating> Ratings { get; set; } = new List<BookUserRating>();
    }

    public class EmptyInputException : ApplicationException
    { 
        public EmptyInputException(string msg) : base(msg) { }  
    }
    public class NullInputException : ApplicationException
    {
        public NullInputException(string msg) : base(msg) { }
    }
}

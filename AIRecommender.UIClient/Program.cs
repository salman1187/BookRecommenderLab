using AIRecommender.CoreEngine;
using AIRecommender.DataAggregator;
using AIRecommender.DataLoader;
using AIRecommender.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Configuration;
using System.Threading.Tasks;
using System.Reflection;

namespace AIRecommender.UIClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AIRecommendationEngine engine = new AIRecommendationEngine();

            IList<Book> booksRecommended = engine.Recommend(new Preference { Age = 25, State = "new york", ISBN = "0060973129" }, 10); 

            if (booksRecommended.Count == 0)
            {
                Console.WriteLine("No books to be recommended");
                return;
            }
            
            Console.WriteLine("\nRecommended Books are: \n");
            foreach (Book book in booksRecommended)
            {
                Console.WriteLine($"{book.BookTitle} - {book.ISBN}");
            }
            
        }
    }
    public class AIRecommendationEngine
    {
        IRecommender aiRecommender;
        public AIRecommendationEngine()
        {
            aiRecommender = new PearsonRecommender();
        }
        public IList<Book> Recommend(Preference preference, int limit)
        {
            DataLoaderFactory factory = DataLoaderFactory.Instance;
            IDataLoader loadData = factory.CreateDataLoader();
            BookDetails bookDetails = loadData.Load();
            //Console.WriteLine("loading done");
           
            //list of ratings of book in preference
            List<int> PrefISBNRatings = new List<int>();    
            foreach(BookUserRating ratings in  bookDetails.Ratings)
            {
                if((preference.ISBN).Equals(ratings.ISBN))
                {
                    PrefISBNRatings.Add(int.Parse(ratings.Rating));
                }
            }
            //dictionary of all relevant books and their ratings
            AIRecommender.DataAggregator.IRatingsAggregator aggregator = new RatingsAggregator();
            Dictionary<string, List<int>> relevantBooks = aggregator.Aggregate(bookDetails, preference);
            Dictionary<string, double> bookCorrelation = new Dictionary<string, double>();


            //Console.WriteLine($"relevant books: {relevantBooks.Count}");
            foreach (var books in relevantBooks)
            {
                bookCorrelation.Add(books.Key, aiRecommender.GetCorrelation(PrefISBNRatings, books.Value)); 
            }
            List<KeyValuePair<string, double>> list = bookCorrelation.ToList();
            list.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            List<string> finalISBNlist = new List<string>();
            if(list.Count > limit)
            {
                for (int i = 0; i <= limit; i++)
                    finalISBNlist.Add(list[i].Key);
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                    finalISBNlist.Add(list[i].Key);
            }
            
            //but need to return the book and not the isbn :(
            Dictionary<string, Book> ISBNToBook = bookDetails.TheBooks.ToDictionary(book => book.ISBN);
            List<Book> ans = new List<Book>();
            Console.WriteLine("FINAL BOOKS CORRELATION: ");
            foreach (string s in  finalISBNlist)
            {
                if(ISBNToBook.ContainsKey(s))
                {
                    ans.Add(ISBNToBook[s]);
                    Console.WriteLine(bookCorrelation[s]);
                }
            }
            return ans; 
        }

    }
}

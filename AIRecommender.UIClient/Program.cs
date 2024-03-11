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
using AIRecommender.DataCacher;

namespace AIRecommender.UIClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AIRecommendationEngine engine = new AIRecommendationEngine();

            while(true)
            {
                Console.WriteLine("Enter Age Preferece: ");
                int age = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter State Preference: ");
                string state = Console.ReadLine();

                Stopwatch sw = Stopwatch.StartNew();
                IList<Book> booksRecommended = engine.Recommend(new Preference { Age = age, State = state, ISBN = "0060973129" }, 10);

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
                Console.WriteLine($"\n Time Taken: {sw.ElapsedMilliseconds}");

                Console.WriteLine("Enter 0 to exit");
                int flag = int.Parse(Console.ReadLine());
                if (flag == 0)
                    break;
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
            BookDataService dataService = new BookDataService();
            BookDetails bookDetails = dataService.GetBookDetails();
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
        public class BookDataService
        {
            public BookDetails GetBookDetails()
            {
                if(MemDataCacher.Instance.GetData() != null)
                    return MemDataCacher.Instance.GetData();

                DataLoaderFactory factory = DataLoaderFactory.Instance;
                IDataLoader loadData = factory.CreateDataLoader();
                BookDetails bookDetails = loadData.Load();

                MemDataCacher.Instance.SetData(bookDetails);

                return bookDetails;
            }
        }

    }
}

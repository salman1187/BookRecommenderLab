using AIRecommender.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRecommender.DataAggregator
{
    public class Preference
    {
        public string ISBN { get; set; }
        public string State { get; set; }
        public int? Age { get; set; }
    }
    public interface IRatingsAggregator
    {
        Dictionary<string, List<int>> Aggregate(BookDetails bookDetails, Preference preference);
    }
    public class RatingsAggregator : IRatingsAggregator
    {
        public Dictionary<string, List<int>> Aggregate(BookDetails bookDetails, Preference preference)
        {
            Dictionary<string, List<int>> bookRatingList = new Dictionary<string, List<int>>();
            foreach (User u in bookDetails.Users)
            {
                if (AgeGroup(u.Age).Equals(AgeGroup(preference.Age)) && (u.State).Equals(preference.State))
                {
                    foreach (BookUserRating ratings in u.Ratings)
                    {
                        // Check if the key exists in the dictionary before accessing it
                        if (!bookRatingList.ContainsKey(ratings.ISBN))
                        {
                            bookRatingList[ratings.ISBN] = new List<int>();
                        }
                        bookRatingList[ratings.ISBN].Add(int.Parse(ratings.Rating));

                    }
                }
            }

            return bookRatingList;
        }
        private string AgeGroup(int? age)
        {
            if (age <= 16)
                return "Teen";
            if (age <= 30)
                return "Young";
            if (age <= 50)
                return "Mid";
            if (age <= 60)
                return "Old";
            else
                return "Senior";
        }
    }
}

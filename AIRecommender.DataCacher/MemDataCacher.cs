using AIRecommender.Entities;
using StackExchange.Redis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace AIRecommender.DataCacher
{
    public interface IDataCacher
    {
        BookDetails GetData();
        void SetData(BookDetails bookDetails);
    }
    public class MemDataCacher : IDataCacher
    {
        private MemDataCacher() { }
        public static readonly MemDataCacher Instance = new MemDataCacher();
        public BookDetails data = null; 
        public BookDetails GetData()
        {
            return data;
        }
        public void SetData(BookDetails bookDetails)
        {
            data = bookDetails; 
        }
    }
    //public class RedisDataCacher : IDataCacher
    //{
    //    private static readonly RedisClient redisClient = new RedisClient("localhost");
    //    public BookDetails GetData()
    //    {
    //        return redisClient.Get<BookDetails>("BookDetails");
    //    }
    //    public void SetData(BookDetails bookDetails)
    //    {
    //        redisClient.Set("BookDetails", bookDetails);
    //    }
    //}
}

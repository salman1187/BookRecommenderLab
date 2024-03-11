using AIRecommender.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}

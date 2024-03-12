using AIRecommender.DataCacher;
using AIRecommender.DataLoader;
using AIRecommender.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRecommender.UIClient
{
    public class BookDataService
    {
        public BookDetails GetBookDetails()
        {
            if (MemDataCacher.Instance.GetData() != null)
                return MemDataCacher.Instance.GetData();

            DataLoaderFactory factory = DataLoaderFactory.Instance;
            IDataLoader loadData = factory.CreateDataLoader();
            BookDetails bookDetails = loadData.Load();

            MemDataCacher.Instance.SetData(bookDetails);

            return bookDetails;
        }
    }
}

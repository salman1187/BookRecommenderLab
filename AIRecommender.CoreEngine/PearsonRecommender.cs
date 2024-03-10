using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRecommender.Entities;

namespace AIRecommender.CoreEngine
{
    public interface IRecommender
    {
        double GetCorrelation(List<int> baseData, List<int> otherData);
    }
    public class PearsonRecommender : IRecommender
    {
        public double GetCorrelation(List<int> baseData, List<int> otherData)
        {
            if (baseData == null || otherData == null) { 
                throw new NullInputException("Input data cannot be null");
            }
            if (baseData.Count == 0 || otherData.Count == 0) {
                throw new EmptyInputException("Input data cannot be empty");
            }
            //base array is bigger
            if (baseData.Count > otherData.Count)
                IncreaseOtherArray(baseData, otherData);
            //base array is smaller
            if (baseData.Count < otherData.Count)
                TrimOtherArray(baseData, otherData);

            //no 0s in the arrays
            ZeroRemover(baseData, otherData);

            double meanBase = Mean(baseData);
            double meanOther = Mean(otherData);

            double sumProduct = 0.0;
            double sumBaseSquared = 0.0;
            double sumOtherSquared = 0.0;

            for (int i = 0; i < baseData.Count; i++)
            {
                double deviationBase = baseData[i] - meanBase;
                double deviationOther = otherData[i] - meanOther;

                sumProduct += deviationBase * deviationOther;
                sumBaseSquared += deviationBase * deviationBase;
                sumOtherSquared += deviationOther * deviationOther;
            }

            double correlation = sumProduct / Math.Sqrt(sumBaseSquared * sumOtherSquared);

            return correlation;
        }

        private double Mean(List<int> data)
        {
            double sum = 0.0;
            foreach (int value in data)
            {
                sum += value;
            }
            return sum / data.Count;
        }

        private void ZeroRemover(List<int> a, List<int> b)
        {
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i] == 0 || b[i] == 0)
                {
                    a[i]++; b[i]++;
                }
            }
        }
        private void IncreaseOtherArray(List<int> a, List<int> b)
        {
            int x = a.Count - b.Count;
            for (int i = 0; i < x; i++)
                b.Add(1);
        }
        private void TrimOtherArray(List<int> a, List<int> b)
        {
            int x = b.Count - a.Count;
            b.RemoveRange(a.Count, x);
        }
    }
}

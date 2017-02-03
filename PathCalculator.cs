using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_V8
{
    public class PathCalculator
    {
        public int calculateDistance(List<int> path)
        {
            int totalDist = 0;
            for (int i=0; i<path.Count-1; i++)
            {
                totalDist += Params.distances[path[i], path[i + 1]];
            }
            return totalDist;
        }
        public double calculateProfit(List<int>path)
        {
            double profit = 0;
            foreach (int i in path)
            {
                profit += Params.profits[i];
            }
            return profit;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_V8
{
    public class FileReader
    {
        public int[,] GetDataFromFile(string file, out int numberOfTowns, out int maxLength, out double[] profits )
        {
            List<Town> towns = new List<Town>();
            towns.Add(new Town(-1, -1, -1));
            using (StreamReader inFile = new StreamReader(file))
            {
                string[] t = inFile.ReadLine().Split(' ');
                numberOfTowns = Convert.ToInt32(t[0]);
                maxLength = Convert.ToInt32(t[1]);
                for (int i=0; i < numberOfTowns; i++)
                {
                    t = inFile.ReadLine().Split(' ');
                    double x, y, p;
                    double.TryParse(t[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                    double.TryParse(t[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                    double.TryParse(t[2], NumberStyles.Any, CultureInfo.InvariantCulture, out p);

                    towns.Add(new Town(x, y, p));
                    if (i == 0)
                    {
                        towns[0].x = x;
                        towns[0].y = y;
                        towns[0].profit = p;
                    }
                }
            }
            int[,] dist = new int[numberOfTowns + 1, numberOfTowns + 1];
            profits = new double[numberOfTowns + 1];
            for (int i=1; i<numberOfTowns+1; i++)
            {
                for (int j=1; j<numberOfTowns+1; j++)
                {
                    int distance = towns[i].calculateDistanceToOtherTown(towns[j]);
                    dist[i, j] = distance;
                    dist[j, i] = distance;
                }
                profits[i] = towns[i].profit;
            }
            return dist;
        }
    }
}

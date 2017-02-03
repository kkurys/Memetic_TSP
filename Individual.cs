using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_V8
{

    public class Individual
    {
        #region properties
        public List<int> path;
        public double profit { get; set; }
        public int length { get; set; }
        public int Count
        {
            get
            {
                return path.Count;
            }
        }
        public double fitness
        {
            get
            {
                if (length > 1.3 * Params.maxLength)
                    return -1;
                else
                    return (profit * profit * profit / length)  * (Params.maxLength * profit / length)*(Params.maxLength * profit / length) * (Params.maxLength / length) * (Params.maxLength / length);
            }
        }
        public int this[int index]
        {
            get
            {
                return path[index];
            }
        }

        #endregion
        #region constructors
        public Individual()
        {
            path = new List<int>();
            profit = 0;
            length = 0;
        }
        public Individual(Individual I)
        {
            path = new List<int>(I.path);
            profit = I.profit;
            length = I.length;
        }
        #endregion
        #region generator
        public void generateFixedIndividual(int startingTown)
        {
            HashSet<int> availableTowns = new HashSet<int>();
            ContainerForDuos bestTowns = new ContainerForDuos(5);
            PathCalculator calc = new PathCalculator();

            int pathDistance = 0;
            for (int i = 1; i <= Params.numberOfTowns; i++)
            {
                if (i == startingTown)
                    continue;
                availableTowns.Add(i);
            }
            List<int> newPath = new List<int>();
            newPath.Add(startingTown);

            profit = 0;
            profit += Params.profits[startingTown];
            do
            {
                int k = Params.rand.Next(2,4);
                bestTowns = new ContainerForDuos(k);
                foreach (int i in availableTowns)
                {
                    double value = calculateValue(newPath[newPath.Count - 1], i, pathDistance, startingTown);
                    if (value != -1)
                    {
                        bestTowns.Add(new Duo(value, i));
                    }
                }
                if (bestTowns.Count == 0)
                {
                    pathDistance += Params.distances[newPath[newPath.Count - 1], startingTown];
                    newPath.Add(startingTown);
                    break;
                }
                else
                {
                    int t = Params.rand.Next(bestTowns.Count - 1);
                    pathDistance += Params.distances[newPath[newPath.Count - 1], bestTowns.town(t)];
                    newPath.Add(bestTowns.town(t));
                    profit += Params.profits[bestTowns.town(t)];
                    availableTowns.Remove(bestTowns.town(t));
                }
            }
            while (newPath[newPath.Count - 1] != startingTown);
            length = pathDistance;
            path = newPath;
            insertCapital();
            partialTwoOpt();
            evaluatePath();
        }
        public void insertCapital()
        {
            if (path.Contains(1) || path.Count == 1) return;
            int bestInsertionPoint = -1;
            int minimalDistanceGain = int.MaxValue;
            for (int i = 1; i <= path.Count - 2; i++)
            {
                if (Params.distances[i - 1, i] - Params.distances[path[i - 1], 1] - Params.distances[1, i] < minimalDistanceGain)
                {
                    bestInsertionPoint = i;
                    minimalDistanceGain = Params.distances[i - 1, i] - Params.distances[path[i - 1], 1] - Params.distances[1, i];
                }
            }
             path.Insert(bestInsertionPoint, 1);
        }
        double calculateValue(int previousTown, int town, int currentPathLength, int finalTown)
        {
            if (currentPathLength + Params.distances[previousTown, town] + Params.distances[town, finalTown] < Params.maxLength)
            {
                return (Params.profits[town]) / Params.distances[previousTown, town];
            }
            else
            {
                return -1;
            }

        }
        #endregion
        #region 2opt

        public void twoOpt()
        {
            bool startAgain = false;
            PathCalculator calc = new PathCalculator();
            int iterations = 0;
            do
            {
                int bestDistance = calc.calculateDistance(path);
                startAgain = false;
                for (int i = 1; i < path.Count - 1; i++)
                {
                    if (startAgain) break;
                    for (int j = i + 1; j < path.Count - 2; j++)
                    {
                        int dist = Params.distances[path[i - 1], path[i]] + Params.distances[path[j], path[j + 1]] - Params.distances[path[i - 1], path[j]] - Params.distances[path[i], path[j + 1]];
                        if (dist > 0)
                        {
                            path = modifyPath(path, i, j);
                            bestDistance -= dist;
                            startAgain = true;
                            break;
                        }
                    }
                }
                iterations++;
            } while (startAgain);
        }
        public void partialTwoOpt()
        {
            bool startAgain = false;
            PathCalculator calc = new PathCalculator();
            int iterations = 0;
            int bestDistance = length;
            do
            {

                startAgain = false;
                for (int i = 1; i < path.Count - 1; i++)
                {
                    if (startAgain) break;
                    for (int j = i + 1; j < path.Count - 2; j++)
                    {
                        int dist = Params.distances[path[i - 1], path[i]] + Params.distances[path[j], path[j + 1]] - Params.distances[path[i - 1], path[j]] - Params.distances[path[i], path[j + 1]];
                        if (dist > 0)
                        {
                            path = modifyPath(path, i, j);
                            bestDistance -= dist;
                            startAgain = true;
                            break;
                        }
                    }
                }
                iterations++;
            } while (startAgain && iterations < 5);
        }
        public List<int> modifyPath(List<int> path, int lT, int rT)
        {
            List<int> modifiedPath = new List<int>(path);
            int tmp;
            while (lT < rT)
            {
                tmp = modifiedPath[lT];
                modifiedPath[lT] = modifiedPath[rT];
                modifiedPath[rT] = tmp;
                lT++;
                rT--;
            }
            return modifiedPath;
        }
        #endregion
        public void evaluatePath()
        {
            length = 0;
            profit = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                length += Params.distances[path[i], path[i + 1]];
                profit += Params.profits[path[i]];
            }
        }


        #region writing
        public void printIndividual()
        {
            Console.WriteLine("Profit: " + profit + " Dist: " + length + " Fitness: " + fitness);
            foreach (int i in path)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
        }
        public void writeIndividualToFile(StreamWriter sw)
        {
            sw.WriteLine(profit);
            foreach (int i in path)
            {
                sw.Write(i + " ");
            }
            sw.WriteLine();
        }
        #endregion
    }
}

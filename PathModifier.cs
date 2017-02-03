using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_V8
{
    static class PathModifier
    {

        public static HashSet<int> aT;

        #region tryinserting
        public static List<int> tryInserting(List<int> path)
        {
            List<int> currentPath = new List<int>(path);

            HashSet<int> availableTowns = new HashSet<int>(aT);
            PathCalculator PO = new PathCalculator();
            int currentPathLength = PO.calculateDistance(currentPath);
            double currentProfit = 0;
            foreach (int i in currentPath)
            {
                currentProfit += Params.profits[i];
                availableTowns.Remove(i);
            }
            double bestPossibleGain = 0;
            int bestPossibleGainIndex = -1;
            int bestPossibleGainTown = -1;
            int bestPossibleGainLengthInc = 0;
            int iterations = 0;
            do
            {
                iterations++;
                bestPossibleGain = 0;
                bestPossibleGainIndex = 0;
                bestPossibleGainTown = -1;
                foreach (int i in availableTowns)
                {
                    for (int x = 0; x < currentPath.Count - 1; x++)
                    {
                        if (currentPathLength + Params.distances[currentPath[x], i] + Params.distances[i, currentPath[x + 1]] - Params.distances[currentPath[x], currentPath[x + 1]] <= Params.maxLength)
                        {

                            if (Params.profits[i] > bestPossibleGain)
                            {

                                bestPossibleGainLengthInc = Params.distances[currentPath[x], i] + Params.distances[i, currentPath[x + 1]] - Params.distances[currentPath[x], currentPath[x + 1]];
                                bestPossibleGain = Params.profits[i];
                                bestPossibleGainIndex = x;
                                bestPossibleGainTown = i;
                            }
                        }
                    }
                }
                if (bestPossibleGainTown != -1)
                {
                    currentPath.Insert(bestPossibleGainIndex + 1, bestPossibleGainTown);
                    currentProfit += bestPossibleGain;
                    availableTowns.Remove(bestPossibleGainTown);
                    currentPathLength = PO.calculateDistance(currentPath);
                }

            } while (bestPossibleGain != 0 && currentPathLength <= Params.maxLength && iterations < 1);

            return currentPath;
        }
        #endregion
        #region tryswapping
        public static List<int> trySwapping(List<int> path)
        {
            List<int> currentPath = new List<int>(path);
            HashSet<int> availableTowns = new HashSet<int>(aT);
            PathCalculator PO = new PathCalculator();
            List<int> added = new List<int>();
            List<int> removed = new List<int>();
            int currentPathLength = PO.calculateDistance(currentPath);
            double currentProfit = 0;

            foreach (int i in currentPath)
            {
                currentProfit += Params.profits[i];
                availableTowns.Remove(i);
            }
            for (int i = 2; i < currentPath.Count - 1; i++)
            {
                if (currentPath[i] == 1) continue;
                foreach (int k in availableTowns)
                {
                    if (Params.profits[k] >= Params.profits[path[i]])
                    {
                        if (Params.profits[k] == Params.profits[path[i]])
                        {
                            if (Params.distances[currentPath[i - 1], currentPath[i]] + Params.distances[currentPath[i], currentPath[i + 1]] > Params.distances[currentPath[i - 1], k] + Params.distances[k, currentPath[i + 1]])
                            {
                                added.Add(currentPath[i]);
                                removed.Add(k);
                                currentPath[i] = k;
                            }
                        }
                        else
                        {
                            if (currentPathLength - Params.distances[currentPath[i - 1], currentPath[i]] - Params.distances[currentPath[i], currentPath[i + 1]] + Params.distances[currentPath[i - 1], k] + Params.distances[k, currentPath[i + 1]] <= Params.maxLength)
                            {
                                added.Add(path[i]);
                                removed.Add(k);
                                currentPath[i] = k;
                            }
                        }
                    }
                }
                foreach (int x in added)
                {
                    availableTowns.Add(x);
                }
                foreach (int x in removed)
                {
                    availableTowns.Remove(x);
                }
                added.Clear();
                removed.Clear();

            }
            return currentPath;
        }
        #endregion
        #region tryexch
        public static void tryExchanging(Individual I)
        {
            HashSet<int> availableTowns = new HashSet<int>(aT);
            foreach (int i in I.path)
            {
                availableTowns.Remove(i);
            }
            double bestProfitGained = -1;
            int indexOfTownToSwap = -1, townToSwap = -1;

            for (int i = 1; i < I.Count - 1; i++)
            {
                if (I[i] == 1) continue;
                foreach (int z in availableTowns)
                {
                    double currentProfitGained = Params.profits[z] - Params.profits[I[i]];
                    if (currentProfitGained > bestProfitGained && I.length - Params.distances[I[i], I[i - 1]] - Params.distances[I[i], I[i + 1]] + Params.distances[I[i - 1], z] + Params.distances[z, I[i + 1]] <= Params.maxLength)
                    {
                        bestProfitGained = currentProfitGained;
                        indexOfTownToSwap = i;
                        townToSwap = z;
                    }
                }
            }
            if (indexOfTownToSwap != -1)
                I.path[indexOfTownToSwap] = townToSwap;
        }
        #endregion
        #region trymoving
        public static List<int> tryMoving(List<int> path)
        {
            List<int> bestPath = new List<int>(path);
            PathCalculator calc = new PathCalculator();
            double bestPathProfit;
            double newProfit = 0;
            int bestPathDist;
            evaluatePath(bestPath, out bestPathDist, out bestPathProfit);
            for (int i = 1; i < bestPath.Count - 2; i++)
            {
                for (int j = 1; j < bestPath.Count - 3; j++)
                {
                    if (i == j) continue;
                    int newDist = bestPathDist;
                    newProfit = 0;

                    if (j == i + 1)
                    {
                        newDist = newDist - Params.distances[bestPath[i - 1], bestPath[i]] + Params.distances[bestPath[i - 1], bestPath[j]] + Params.distances[bestPath[i], bestPath[j + 1]] - Params.distances[bestPath[j], bestPath[j + 1]];
                    }
                    else if (i == j + 1)
                    {
                        newDist += -Params.distances[bestPath[j - 1], bestPath[j]] + Params.distances[bestPath[j - 1], bestPath[j + 1]] + Params.distances[bestPath[j], bestPath[j + 1]];
                    }
                    else if (i > j)
                    {
                        newDist = newDist - Params.distances[bestPath[i - 1], bestPath[i]] - Params.distances[bestPath[i], bestPath[i + 1]]
                                   - Params.distances[bestPath[j], bestPath[j + 1]]
                                   + Params.distances[bestPath[i - 1], bestPath[i + 1]] + Params.distances[bestPath[j], bestPath[i]] + Params.distances[bestPath[i], bestPath[j + 1]];
                    }
                    else if (j > i)
                    {
                        newDist += -Params.distances[bestPath[j - 1], bestPath[j]] - Params.distances[bestPath[j], bestPath[j + 1]]
                                   - Params.distances[bestPath[i - 1], bestPath[i]]
                                   + Params.distances[bestPath[j - 1], bestPath[j + 1]] + Params.distances[bestPath[i - 1], bestPath[j]] + Params.distances[bestPath[j], bestPath[i]];
                    }
                    if (newDist >= Params.maxLength) continue;
                    List<int> newPath = new List<int>(bestPath);

                    int tmp = newPath[i];
                    newPath.Remove(newPath[i]);
                    newPath.Insert(j, tmp);
                    newPath = tryInserting(newPath);
                    evaluatePath(newPath, out newDist, out newProfit);
                    if (newProfit > bestPathProfit)
                    {
                        bestPath = newPath;
                        bestPathProfit = newProfit;
                        bestPathDist = newDist;
                    }
                }
            }

            return bestPath;
        }
        #endregion
        #region tryRemoving
        static public void tryRemoveChange(Individual I)
        {
            HashSet<int> availableTowns = new HashSet<int>(aT);
            ContainerForDuos townsToRemove = new ContainerForDuos(Params.rand.Next(1, I.Count / 2));

            for (int i = 1; i < I.Count - 1; i++)
            {
                if (I[i] == 1) continue;
                double value = (Params.distances[I[i - 1], I[i]] + Params.distances[I[i], I[i + 1]]) / Params.profits[I[i]] * Params.profits[I[i]] * Params.profits[I[i]];
                if (townsToRemove.Count < townsToRemove.size)
                {
                    townsToRemove.Add(new Duo(value, I[i]));
                }
                else
                {
                    if (value > townsToRemove[townsToRemove.size - 1].value)
                    {
                        townsToRemove.Add(new Duo(value, I[i]));
                    }
                }
            }
            foreach (Duo D in townsToRemove)
            {
                I.path.Remove(D.index);
            }
            I.evaluatePath();
            I.path = tryInserting(I.path);
            I.evaluatePath();
        }
        #endregion
        public static void tryMutate(Individual I)
        {
            List<int> availableTowns = new List<int>(aT);
            foreach (int i in I.path)
            {
                availableTowns.Remove(i);
            }
            int maxValueTown = 0;
            double maxValueProfit = 0;
            for (int i = 0; i < availableTowns.Count; i++)
            {
                if (Params.profits[availableTowns[i]] > maxValueProfit)
                {
                    maxValueTown = i;
                    maxValueProfit = Params.profits[availableTowns[i]];
                }
            }
            int z = Params.rand.Next(I.Count - 2) + 2;

            I.path[z] = availableTowns[maxValueTown];

        }
        public static void evaluatePath(List<int> path, out int length, out double profit)
        {
            length = 0;
            profit = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                length += Params.distances[path[i], path[i + 1]];
                profit += Params.profits[path[i]];
            }
        }


        public static void getTownsHS()
        {
            aT = new HashSet<int>();
            for (int i = 2; i <= Params.numberOfTowns; i++)
            {
                aT.Add(i);
            }
        }

    }
}

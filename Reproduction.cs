using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_V8
{
    public class Reproduction
    {
        public Individual crossOver(Individual parent1, Individual parent2)
        {
            Individual child = new Individual();
            int point = parent1.Count > parent2.Count ? Params.rand.Next(parent2.Count - 2) + 2 : Params.rand.Next(parent1.Count - 2) + 2;
            child.path.Add(parent1.path[0]);
            child.length = 0;
            child.profit = 0;
            bool[] wasUsed = new bool[Params.numberOfTowns + 1];
            wasUsed[parent1[0]] = true;
            for (int i = 1; i < point; i++)
            {
                child.path.Add(parent1.path[i]);
                child.profit += Params.profits[parent1.path[i]];
                child.length += Params.distances[child.path[i - 1], child.path[i]];
                wasUsed[parent1.path[i]] = true;
            }
            // child.path.Contains(parent2.path[i])
            for (int i = point; i < parent2.path.Count - 1; i++)
            {
                if (!wasUsed[parent2[i]] && child.length + Params.distances[child.path[child.Count-1], parent2.path[i]] + Params.distances[child.path[0], parent2.path[i]] <= 1.03 * Params.maxLength)
                {
                    child.path.Add(parent2.path[i]);
                    child.profit += Params.profits[parent2.path[i]];
                    child.length += Params.distances[child.path[child.Count-1], child.path[child.Count-2]];
                }
            }
            child.path.Add(child.path[0]);
            child.length += Params.distances[child.path[child.Count - 2], child.path[child.Count - 1]];
            child.insertCapital();

            double chance = Params.rand.NextDouble();
            if (chance > 0.999)
            {
                PathModifier.tryRemoveChange(child);
            }
            else if (chance < 0.05 && child.length < Params.maxLength)
            {
                PathModifier.tryMutate(child);
            }
            else if (chance < 0.03 && child.length < Params.maxLength)
            {
                PathModifier.tryExchanging(child);
            }
            else if (chance > 0.9965)
            {
                child.path = PathModifier.tryMoving(child.path);
            }
            else if (chance > 0.6 && chance < 0.63)
            {
                child.path = PathModifier.trySwapping(child.path);
            }
            else if (chance > 0.7 && chance < 0.73)
            {
                child.partialTwoOpt();
            }
            child.evaluatePath();
            int similarityToParent1 = 0, similarityToParent2 = 0;
            double pSimilarityToParent1 = 0, pSimilarityToParent2 = 0;
            for (int i = 0; i < child.Count; i++)
            {
                if (i < parent1.Count)
                {
                    if (child.path[i] == parent1.path[i])
                    {
                        similarityToParent1++;
                    }
                }
                if (i < parent2.Count)
                {
                    if (child.path[i] == parent2.path[i])
                    {
                        similarityToParent2++;
                    }
                }
                if (i >= parent2.Count && i >= parent1.Count) break;
            }
            pSimilarityToParent1 = similarityToParent1 / (child.Count);
            pSimilarityToParent2 = similarityToParent2 / (child.Count);
            if (pSimilarityToParent1 >= pSimilarityToParent2)
            {
                if (parent1.fitness > child.fitness)
                {
                    return parent1;
                }

            }
            if (pSimilarityToParent2 >= pSimilarityToParent1)
            {
                if (parent2.fitness > child.fitness)
                {
                    return parent2;
                }
            }

            return child;
        }
    }
}

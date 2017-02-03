using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_V8
{
    class Program
    {
        static void Main(string[] args)
        {
            Params.rand = new Random();
            Params.GetParams();
            PathModifier.getTownsHS();
            Params.bestOne = new Individual();
            Params.bestOne.generateFixedIndividual(1);
            Individual I = new Individual();
            Population oldPopulation, newPopulation;
            Reproduction breed = new Reproduction();
            oldPopulation = new Population(100);
            double chance;
            
            for (int i = 0; i < 100; i++)
            {
                int startingTown = Params.rand.Next(Params.numberOfTowns)+1;
                I = new Individual();
                I.generateFixedIndividual(startingTown);
                oldPopulation.Add(I);
            }
            for (int generation = 0; generation < 37; generation++)
            {
                newPopulation = new Population(100);
                for (int i = 0; i < 100; i++)
                {
                    chance = Params.rand.NextDouble();
                    for (int j = i+1 ; j < 100; j++)
                    {
                        if (chance < (1 - (i/2 + j/2)/100))
                        {
                            I = breed.crossOver(oldPopulation[i], oldPopulation[j]);
                            if (!newPopulation.population.Contains(I))
                                newPopulation.Add(I);
                        }

                    }
                }
                 oldPopulation = newPopulation;
                
            }
            Params.bestOne.printIndividual();
            StreamWriter sw = new StreamWriter("bestPath.txt");
            Params.bestOne.writeIndividualToFile(sw);
            sw.Close();
            Console.ReadKey();
        }
    }
}

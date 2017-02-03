using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_V8
{
    public static class Params
    {
        public static int numberOfTowns;
        public static int maxLength;
        public static int[,] distances;
        public static double[] profits;
        public static Random rand;
        public static Individual bestOne;
        public static void GetParams()
        {
            FileReader fileIn = new FileReader();
            distances = fileIn.GetDataFromFile("IN3.txt", out numberOfTowns, out maxLength, out profits);
        }
        public static void PrintDistances()
        {

            for (int i=1; i<=numberOfTowns; i++)
            {
                for (int j=1; j<=numberOfTowns; j++)
                {
                    Console.Write(distances[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

    }
}

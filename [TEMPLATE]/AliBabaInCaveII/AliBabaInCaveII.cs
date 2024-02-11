using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class AliBabaInCaveII
    {
        #region YOUR CODE IS HERE

        #region FUNCTION#1: Calculate the Value
        //Your Code is Here:
        //==================
        /// <summary>
        /// Given the Camels possible load and N items, each with its weight, profit and number of instances, 
        /// Calculate the max total profit that can be carried within the given camels' load
        /// </summary>
        /// <param name="camelsLoad">max load that can be carried by camels</param>
        /// <param name="itemsCount">number of items</param>
        /// <param name="weights">weight of each item [ONE-BASED array]</param>
        /// <param name="profits">profit of each item [ONE-BASED array]</param>
        /// <param name="instances">number of instances for each item [ONE-BASED array]</param>
        /// <returns>Max total profit</returns>
        /// 




        public static int[,] MaxProf(int[,] dp, int[] prof, int[] weight, int[] inst, int items_Count, int camels)
        {
            for (int i = 1; i <= items_Count; i++)
                MAxProfitDP(dp, prof, weight, inst, camels, i);
            return dp;
        }

        public static void MAxProfitDP(int[,] dp, int[] prof, int[] weight, int[] inst, int camels, int i)
        {
            for (int j = 1; j <= camels; j++)
                for (int k = 0; k <= inst[i]; k++)
                    if (weight[i] * k <= j)
                        dp[i, j] = Math.Max(dp[i, j], dp[i - 1, j - (weight[i] * k)] + (prof[i] * k));
        }

        public static int SolveValue(int camelsLoad, int itemsCount, int[] weights, int[] profits, int[] instances)
        {
            int[,] dp_ = new int[itemsCount + 1, camelsLoad + 1];
            
            return MaxProf(dp_, profits, weights, instances, itemsCount, camelsLoad)[itemsCount, camelsLoad];
        }

        #endregion



        public static Tuple<int, int>[] ConstructSolution(int camelsLoad, int itemsCount, int[] weights, int[] profits, int[] instances)
        {
            List<Tuple<int, int>> solution = new List<Tuple<int, int>>();


            int[,] count = new int[itemsCount + 1, camelsLoad + 1];
            int k;

            int[,] dpp = new int[itemsCount + 1, camelsLoad + 1];


            for (int i = 1; i <= itemsCount; i++)
                ConstructSolutionDP(camelsLoad, weights, profits, instances, dpp, count, i);

            if (dpp[itemsCount, camelsLoad] == 0)
                return null;

           

            while (itemsCount > 0 && camelsLoad > 0)
            {
                k = count[itemsCount, camelsLoad];

                if (k > 0)
                {
                    solution.Add(Tuple.Create(itemsCount, k));
                    camelsLoad -= (weights[itemsCount] * k);
                }
                itemsCount--;
            }
            solution.Reverse();
            return solution.ToArray();
        }

        public static void ConstructSolutionDP(int camels, int[] weight, int[] prof, int[] inst, int[,] dp, int[,] count, int i)
        {
            int j = 1;
            while (j <= camels)
            {
                int k = 0;
                while (k <= inst[i])
                {
                    if (weight[i] * k <= j && dp[i - 1, j - weight[i] * k] + (prof[i] * k) > dp[i, j])
                    {
                        dp[i, j] = dp[i - 1, j - weight[i] * k] + (prof[i] * k);
                        count[i, j] = k;
                    }
                    k++;
                }
                j++;
            }
        }

        #endregion
       
    }
}


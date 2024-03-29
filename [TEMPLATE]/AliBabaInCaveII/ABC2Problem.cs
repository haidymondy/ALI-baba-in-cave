﻿using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Problem
{

    public class Problem : ProblemBase, IProblem
    {
        #region ProblemBase Methods
        public override string ProblemName { get { return "AliBabaInCaveII"; } }

        public override void TryMyCode()
        {
            int N = 0, L = 0;
            int outputVal, expectedVal;
            Tuple<int,int> [] outputItems;

            {
                N = 4;
                L = 10;
                int[] weights = { 0, 2, 4, 3, 4 };
                int[] profits = { 0, 1, 8, 6, 5 };
                int[] instances = { 0, 2, 2, 2, 2 };
                expectedVal = 20;
                outputVal = AliBabaInCaveII.SolveValue(L,N,weights, profits, instances);
                outputItems = AliBabaInCaveII.ConstructSolution(L, N, weights, profits, instances);
                PrintCase(L, N, weights, profits, instances, outputVal, outputItems, expectedVal);
            }
            {
                N = 3;
                L = 9;
                int[] weights = {0, 1,2,3};
                int[] profits = {0, 5,4,3};
                int[] instances = {0, 2,1,1};
                expectedVal = 17;
                outputVal = AliBabaInCaveII.SolveValue(L, N, weights, profits, instances);
                outputItems = AliBabaInCaveII.ConstructSolution(L, N, weights, profits, instances);
                PrintCase(L, N, weights, profits, instances, outputVal, outputItems, expectedVal);
            }
            {
                N = 4;
                L = 9;
                int[] weights = {0, 1, 3,4,1 };
                int[] profits = {0, 7,5,2,3 };
                int[] instances = {0, 3,3,3,2};
                expectedVal = 32;
                outputVal = AliBabaInCaveII.SolveValue(L, N, weights, profits, instances);
                outputItems = AliBabaInCaveII.ConstructSolution(L, N, weights, profits, instances);
                PrintCase(L, N, weights, profits, instances, outputVal, outputItems, expectedVal);
            }
            {
                N = 2;
                L = 3;
                int[] weights = {0, 4,4 };
                int[] profits = {0, 2,6 };
                int[] instances = {0, 3,3 };
                expectedVal = 0;
                outputVal = AliBabaInCaveII.SolveValue(L, N, weights, profits, instances);
                outputItems = AliBabaInCaveII.ConstructSolution(L, N, weights, profits, instances);
                PrintCase(L, N, weights, profits, instances, outputVal, outputItems, expectedVal);
            }
        }

       

        Thread tstCaseThr;
        bool caseTimedOut ;
        bool caseException;

        protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
        {
            int testCases;
            int output = -1;
            int actualResult = 0;

            Stream s = new FileStream(fileName, FileMode.Open);
            StreamReader br = new StreamReader(s);

            testCases = int.Parse(br.ReadLine());

            int totalCases = testCases;
            int[] correctCases = new int[2];
            int[] wrongCases = new int[2];
            int[] timeLimitCases = new int[2];
            for (int i = 0; i < 2; i++)
            {
                correctCases[i] = 0;
                wrongCases[i] = 0;
                timeLimitCases[i] = 0;
            }
            bool readTimeFromFile = false;
            if (timeOutInMillisec == -1)
            {
                readTimeFromFile = true;
            }
            float maxTime = float.MinValue;
            float avgTime = 0;

            int[] weights = null;
            int[] profits = null;
            int[] instances = null;
            int itemsCount;
            int camelsLoad;
            string[] lineItems;

            for (int i = 1; i <= testCases; i++)
            {
                lineItems = br.ReadLine().Split(' ');
                itemsCount = int.Parse(lineItems[0]);
                camelsLoad = int.Parse(lineItems[1]);
                weights = new int[itemsCount + 1];
                profits = new int[itemsCount + 1];
                instances = new int[itemsCount + 1];
                for (int j = 1; j <= itemsCount; j++)
                {
                    lineItems = br.ReadLine().Split(' ');
                    weights[j] = int.Parse(lineItems[0]);
                    profits[j] = int.Parse(lineItems[1]);
                    instances[j] = int.Parse(lineItems[2]);
                }

                actualResult = int.Parse(br.ReadLine());
                if (readTimeFromFile)
                {
                    timeOutInMillisec = int.Parse(br.ReadLine().Split(':')[1]);
                }

                Console.WriteLine("\n=====================================");
                Console.WriteLine("CASE#{0}: #items = {1}, Load = {2}", i, itemsCount, camelsLoad);
                Console.WriteLine("=====================================");

                for (int c = 0; c < 2; c++)
                {
                    caseTimedOut = true;
                    Stopwatch sw = null;
                    caseException = false;
                    Tuple<int, int>[] outputVals = null;
                    {
                        tstCaseThr = new Thread(() =>
                        {
                            try
                            {
                                sw = Stopwatch.StartNew();
                                if (c == 0)
                                {
                                    output = AliBabaInCaveII.SolveValue(camelsLoad, itemsCount, weights, profits, instances);
                                }
                                else
                                {
                                    outputVals = AliBabaInCaveII.ConstructSolution(camelsLoad, itemsCount, weights, profits, instances);
                                    if (outputVals == null)
                                        output = 0;
                                }
                                sw.Stop();
                                Console.WriteLine("time = {0} ms", sw.ElapsedMilliseconds);
                                //Console.WriteLine("output = {0}", output);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                caseException = true;
                                output = -1;
                                outputVals = null;
                            }
                            caseTimedOut = false;
                        });

                        //StartTimer(timeOutInMillisec);
                        tstCaseThr.Start();
                        tstCaseThr.Join(timeOutInMillisec);
                    }

                    if (caseTimedOut)       //Timedout
                    {
                        tstCaseThr.Abort();
                        Console.WriteLine("Time Limit Exceeded in Case {0} [FUNCTION#{1}].", i, c+1);
                        timeLimitCases[c]++;
                    }
                    else if (caseException) //Exception 
                    {
                        Console.WriteLine("Exception in Case {0} [FUNCTION#{1}].", i, c+1);
                        wrongCases[c]++;
                    }
                    else if (output == actualResult)    //Passed
                    {
                        if (c == 0)
                        {
                            Console.WriteLine("Test Case {0} [FUNCTION#{1}] Passed!", i, c + 1);
                            correctCases[c]++;
                        }
                        else if (CheckOutput(camelsLoad, weights, profits, instances, outputVals, output, actualResult))
                        {
                            Console.WriteLine("Test Case {0} [FUNCTION#{1}] Passed!", i, c + 1);
                            correctCases[c]++;
                        }
                        else
                        {
                            Console.WriteLine("Wrong Answer in Case {0} [FUNCTION#{1}].", i, c + 1);
                            wrongCases[c]++;
                        }
                        //maxTime = Math.Max(maxTime, sw.ElapsedMilliseconds);
                        //avgTime += sw.ElapsedMilliseconds;
                    }
                    else                    //WrongAnswer
                    {
                        Console.WriteLine("Wrong Answer in Case {0} [FUNCTION#{1}].", i, c+1);
                        if (level == HardniessLevel.Easy)
                        {
                            if (output != -1)
                            {
                                PrintCase(camelsLoad, itemsCount, weights, profits, instances, output, outputVals, actualResult, false);
                            }
                            else
                            {
                                Console.WriteLine("Exception is occur");
                            }
                        }
                        wrongCases[c]++;
                    }
                }
            }
            s.Close();
            br.Close();
            for (int c = 0; c < 2; c++)
            {
                Console.WriteLine("EVALUATION OF FUNCTION#{0}:", c+1);
                Console.WriteLine("# correct = {0}", correctCases[c]);
                Console.WriteLine("# time limit = {0}", timeLimitCases[c]);
                Console.WriteLine("# wrong = {0}", wrongCases[c]);
                //Console.WriteLine("\nFINAL EVALUATION (%), AVG TIME, MAX TIME = {0} {1} {2}", Math.Round((float)correctCasesPart1 / totalCases * 100, 0), correctCasesPart1 == 0 ? -1 : Math.Round(avgTime / (float)correctCasesPart1, 2), correctCasesPart1 == 0 ? -1 : maxTime);
                //Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0));
                //Console.WriteLine("AVERAGE EXECUTION TIME (ms) = {0}", Math.Round(avgTime / (float)correctCases, 2));
                //Console.WriteLine("MAX EXECUTION TIME (ms) = {0}", maxTime); 
            }
            Console.WriteLine("\nFINAL EVALUATION: FUNCTION#1 (%), FUNCTION#2 (%) = {0} {1}", Math.Round((float)correctCases[0] / totalCases * 100, 0), Math.Round((float)correctCases[1] / totalCases * 100, 0));

        }

       

        protected override void OnTimeOut(DateTime signalTime)
        {
        }

        public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
        {
            throw new NotImplementedException();

        }

        #endregion

        #region Helper Methods
        private static void PrintNums(Tuple<int, int>[] X)
        {
            int N = X.Length;
            Console.WriteLine("Item#   #Instances");
            Console.WriteLine("=====================");
            for (int j = 0; j < N; j++)
            {
                Console.WriteLine(X[j].Item1 + "    " + X[j].Item2);
            }
            Console.WriteLine();
        }

        private void PrintCase(int L, int N, int[] weights, int[] profits, int[] instances, int outputVal, Tuple<int,int>[] outputItems, int expectedVal, bool check = true)
        {
            Console.WriteLine("camelsLoad = " + L + ", num of items = " + N);
            Console.WriteLine("Weight   Profit  #Instances");
            Console.WriteLine("=============================");
            for (int j = 1; j <= N; j++)
            {
                Console.WriteLine(weights[j] + "    " + profits[j] + "    " + instances[j]);
            }
            Console.WriteLine("expected value = {0}", expectedVal);
            Console.WriteLine("output value = {0}", outputVal);
            if (outputItems != null)
            {
                Console.WriteLine("output items: "); 
                PrintNums(outputItems);
            }

            if (check)
            {
                if (outputVal != expectedVal)
                {
                    Console.WriteLine("WRONG");
                }
                else
                {
                    if (CheckOutput(L, weights, profits, instances, outputItems, outputVal, expectedVal)) Console.WriteLine("CORRECT");
                    else Console.WriteLine("WRONG");
                }
            }
            Console.WriteLine();
        }

        private bool CheckOutput(int camelsLoad, int[] weights, int[] profits, int[] instances, Tuple<int,int>[] outputItems, int outputVal, int expectedVal)
        {
            if (outputItems == null)
            {
                if (outputVal == expectedVal && expectedVal == 0)
                    return true;
                Console.WriteLine("WRONG: no selected items while expected one or more");
                return false;
            }
            int H = outputItems.Length;
            int N = weights.Length;

            if (outputVal != expectedVal)
            {
                Console.WriteLine("WRONG: output value {0} NOT EQUAL expected value {1}", outputVal, expectedVal);
                return false;
            }
            int weightSum = 0;
            int profitSum = 0;
            for (int i = 0; i < H; i++)
            {
                int idx = outputItems[i].Item1;
                int numInstances = outputItems[i].Item2;
                if (numInstances < 1 || numInstances > instances[idx])
                {
                    Console.WriteLine("WRONG: # taken instances ({0}) from item {1} not a valid number", numInstances, idx);
                    return false;
                }
                if (idx < 1 || idx > N)
                {
                    Console.WriteLine("WRONG: {0} not a valid index (remember it's 1-based)", idx);
                    return false;
                }
                weightSum += weights[idx] * numInstances;
                profitSum += profits[idx] * numInstances;
            }

            if (weightSum > camelsLoad)
            {
                Console.WriteLine("WRONG: total weight of returned items ({0}) is GREATER THAN camels load ({1})", weightSum, camelsLoad);
                return false;
            }
            

            if (profitSum != expectedVal)
            {
                Console.WriteLine("WRONG: values sum of returned items ({0}) NOT EQUAL the expected optimal value ({1})", profitSum, expectedVal);
                return false;
            }
                
            return true;
        }
        #endregion
   
    }
}

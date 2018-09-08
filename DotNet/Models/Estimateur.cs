
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace DotNet.Models
{
    static class Estimateur
    {
        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingCov", CallingConvention = CallingConvention.Cdecl)]

        public static extern int WREmodelingCov(
            ref int returnsSize,
            ref int nbSec,
            double[,] secReturns,
            double[,] covMatrix,
            ref int info
        );

        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingCorr", CallingConvention = CallingConvention.Cdecl)]

        public static extern int WREmodelingCorr(
            ref int returnsSize,
            ref int nbSec,
            double[,] secReturns,
            double[,] corrMatrix,
            ref int info
        );

        public static double[,] computeCovarianceMatrix(double[,] returns)
        {
            int dataSize = returns.GetLength(0);
            int nbAssets = returns.GetLength(1);
            double[,] covMatrix = new double[nbAssets, nbAssets];
            int info = 0;
            int res;
            res = WREmodelingCov(ref dataSize, ref nbAssets, returns, covMatrix, ref info);
            if (res != 0)
            {
                if (res < 0)
                    throw new Exception("ERROR: WREmodelingCov encountred a problem. See info parameter for more details");
                else
                    throw new Exception("WARNING: WREmodelingCov encountred a problem. See info parameter for more details");
            }
            return covMatrix;
        }

        public static double[,] computeCorrelationMatrix(double[,] returns)
        {
            int dataSize = returns.GetLength(0);
            int nbAssets = returns.GetLength(1);
            double[,] corrMatrix = new double[nbAssets, nbAssets];
            int info = 0;
            int res;
            res = WREmodelingCorr(ref dataSize, ref nbAssets, returns, corrMatrix, ref info);
            if (res != 0)
            {
                if (res < 0)
                    throw new Exception("ERROR: WREmodelingCov encountred a problem. See info parameter for more details");
                else
                    throw new Exception("WARNING: WREmodelingCov encountred a problem. See info parameter for more details");
            }
            return corrMatrix;
        }

        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingLogReturns", CallingConvention = CallingConvention.Cdecl)]

        public static extern int WREmodelingLogReturns(
            ref int nbValues,
            ref int nbAssets,
            double[,] assetsValues,
            ref int horizon,
            double[,] assetsReturns,
            ref int info
        );

        public static double[,] computeLogReturnsMatrix(double[,] returns)
        {
            int nbValues = returns.GetLength(0);
            int nbAssets = returns.GetLength(1);
            int info = 0;
            int horizon = 1;
            double[,] logReturnsMatrix = new double[nbValues-horizon, nbAssets];
            int res;
            res = WREmodelingLogReturns(ref nbValues, ref nbAssets, returns,ref horizon, logReturnsMatrix, ref info);
            if (res != 0)
            {
                if (res < 0)
                    throw new Exception("ERROR: WREmodelingCov encountred a problem. See info parameter for more details");
                else
                    throw new Exception("WARNING: WREmodelingCov encountred a problem. See info parameter for more details");
            }
            return logReturnsMatrix;
        }


        static double volatilite;
        static double correlation;

        public static List<DataFeed> CutDataFeed(List<DataFeed> dataList, int joursDEstimation, DateTime dateActuelle)
        {
            DateTime dateDebutEstimation = dateActuelle.AddDays(-joursDEstimation);
            List<DataFeed> cutData = new List<DataFeed>();
            foreach (var element in dataList)
            {
                if (element.Date <= dateActuelle && element.Date >= dateDebutEstimation)
                {
                    cutData.Add(element);
                }
            }
            return cutData;

        }


        public static double[,] getCovMatrix(List<DataFeed> dataList, int joursDEstimation, DateTime dateActuelle)
        {
            List<DataFeed> cutData = CutDataFeed(dataList, joursDEstimation, dateActuelle);
            int n = cutData.Count();
            int m = cutData.First().PriceList.Count();
            Console.WriteLine(n);
            double[,] assetsValues = new double[n, m];
            for (int i = 0; i < cutData.Count(); i++)
            {
                for (int j = 0; j < cutData.First().PriceList.Count(); j++)
                {
                    decimal element = cutData[i].PriceList.ElementAt(j).Value;
                    assetsValues[i, j] = (double)element;
                }
            }
            double[,] logReturns = computeLogReturnsMatrix(assetsValues);
            return computeCovarianceMatrix(logReturns);
        }

        public static double[,] getCorrMatrix(List<DataFeed> dataList, int joursDEstimation, DateTime dateActuelle)
        {
            List<DataFeed> cutData = CutDataFeed(dataList, joursDEstimation, dateActuelle);
            int n = cutData.Count();
            int m = cutData.First().PriceList.Count();
            Console.WriteLine(n);
            double[,] assetsValues = new double[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    decimal element = cutData[i].PriceList.ElementAt(j).Value;
                    assetsValues[i, j] = (double) element;
                }
            }
            double[,] logReturns = computeLogReturnsMatrix(assetsValues);
            return computeCorrelationMatrix(logReturns);
        }

        public static void dispMatrix(double[,] myCovMatrix)
        {
            int n = myCovMatrix.GetLength(0);

            Console.WriteLine("Covariance matrix:");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(myCovMatrix[i, j] + "\t");
                }
                Console.Write("\n");
            }
        }

        public static double[,] matrixWithWeight(double[,] matrix, double[] weight)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    matrix[i, j] = weight[i] * weight[j] * matrix[i, j];
            return matrix;
        }

        public static double Volatile(List<DataFeed> dataList, int joursDEstimation, DateTime dateActuelle, double[] weight, IDataFeedProvider dataFeedProvider)
        {
            double[,] covMatrix = getCovMatrix(dataList, joursDEstimation, dateActuelle);
            covMatrix = matrixWithWeight(covMatrix, weight);
            double variancePortefeuille = 0;
            for (int i = 0; i < covMatrix.GetLength(0); i++)
                for (int j = 0; j < covMatrix.GetLength(1); j++)
                    variancePortefeuille += covMatrix[i, j];
            return Math.Sqrt(variancePortefeuille) * Math.Sqrt(dataFeedProvider.NumberOfDaysPerYear);
        }
    }
}
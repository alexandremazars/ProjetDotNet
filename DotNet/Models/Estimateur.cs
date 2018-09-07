using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Types.Sql;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace DotNet.Models
{
    class Estimateur
    {
        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingCov", CallingConvention = CallingConvention.Cdecl)]

        public static extern int WREmodelingCov(
            ref int returnsSize,
            ref int nbSec,
            double[,] secReturns,
            double[,] covMatrix,
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
            double[,] logReturnsMatrix = new double[nbValues, nbAssets];
            int info = 0;
            int horizon = 1;
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


        double volatilite;
        double correlation;
        List<DataFeed> dataList;

        public List<DataFeed> CutDataFeed(List<DataFeed> dataList, int joursDEstimation, DateTime dateActuelle)
        {
            DateTime dateDebutEstimation = dateActuelle.AddDays(-joursDEstimation);
            List<DataFeed> cutData = new List<DataFeed>();
            foreach (var element in dataList)
            {
                if (element.Date <= dateActuelle)
                {
                    cutData.Add(element);
                }
            }
            return cutData;

        }


        public double[,] getCovMatrix(List<DataFeed> dataList, int joursDEstimation, DateTime dateActuelle)
        {
            List<DataFeed> cutData = CutDataFeed(dataList, joursDEstimation, dateActuelle);
            double[,] assetsValues = new double[cutData.Count(), cutData.First().PriceList.Count()];
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
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using SolarSolution.Properties;

namespace SolarSolution
{
    public class NormalConsume : MainWindow
    {
        // public struct rankElectricWork
        // {
        //     public double Price;
        //     public double quantityAllowed;
        //     public double usedPrice;
        //
        //     public double UsedWork()
        //     {
        //         return usedPrice / Price;
        //     }
        // }
        public SortedList<object, rankElectricWork> rankElectricWorkList = new SortedList<object, rankElectricWork>();
        public double consumeMonth = 0;
        public NormalConsume(double consumeMonth)
        {
            this.consumeMonth = consumeMonth;




        }

        private double Athapdiem;
        private double Atrungbinh;
        private double Acaodiem;

        public NormalConsume(double Athapdiem, double Atrungbinh, double Acaodiem)
        {
            this.Athapdiem = Athapdiem;
            this.Atrungbinh = Atrungbinh;
            this.Acaodiem = Acaodiem;

        }


        public void Loaded()
        {
            if (consumeMonth == 0)
            {
                WorkOtherCaulation();
            }
            else
            {
                DevinePriceWork();
            }


        }
        public double AmmountMoneyforEnterprise;
        private void WorkOtherCaulation()
        {
            var rankElectricWork = rankElectricWorkList["Thấp điểm"];
            rankElectricWork.UsedWork = Athapdiem;
            rankElectricWork.usedPrice = rankElectricWork.UsedWork * rankElectricWork.Price;
            rankElectricWorkList["Thấp điểm"] = rankElectricWork;

            rankElectricWork = rankElectricWorkList["Bình thường"];
            rankElectricWork.UsedWork = Atrungbinh;
            rankElectricWork.usedPrice = rankElectricWork.UsedWork * rankElectricWork.Price;
            rankElectricWorkList["Bình thường"] = rankElectricWork;
             
            rankElectricWork = rankElectricWorkList["Cao điểm"];
            rankElectricWork.UsedWork = Acaodiem;
            rankElectricWork.usedPrice = rankElectricWork.UsedWork * rankElectricWork.Price;
            rankElectricWorkList["Cao điểm"] = rankElectricWork;
            AmmountMoneyforEnterprise = rankElectricWorkList["Thấp điểm"].usedPrice + rankElectricWorkList["Bình thường"].usedPrice
                + rankElectricWorkList["Cao điểm"].usedPrice;

            // string[] keyList = new string[3] {"Cao điểm", "Bình thường", "Thấp điểm"};

            // foreach (string key in keyList)
            // {
            //     rankElectricWork = rankElectricWorkList[key];
            //     rankElectricWork.UsedWork = Atrungbinh;
            //     rankElectricWork.usedPrice = rankElectricWork.UsedWork * rankElectricWork.Price;
            //     rankElectricWorkList[key] = rankElectricWork;
            // }

        }

        private void DevinePriceWork()
        {
            double currentConsume = consumeMonth;
            for (double i = 1; i <= rankElectricWorkList.Count; i++)
            {
                var E = (rankElectricWork)rankElectricWorkList[i];
                double moneyEachRank = E.Price * E.quantityAllowed;

                if (currentConsume < moneyEachRank)
                {
                    E.usedPrice = currentConsume;
                    currentConsume = 0;
                    break;
                }
                else
                {
                    currentConsume -= moneyEachRank;
                    E.usedPrice = moneyEachRank;
                }

                rankElectricWorkList[i] = E;




            }

            double index = rankElectricWorkList.Count;
            rankElectricWork El = (rankElectricWork)rankElectricWorkList[index];
            El.usedPrice += currentConsume;
            rankElectricWorkList[index] = El;

        }


    }
}

using System.Collections;
using System.Collections.Generic;

namespace SolarSolution
{
    public class SolarCal : MainWindow
    {
        public double ammountMonney;
        public double Kwp;

        public SortedList<object, rankElectricWork>
            rankElectricWorkPrivate = new SortedList<object, rankElectricWork>();
        public double sellforEVN;
        public double sunnyTime;
        public double SurplusPrice;
        public double SurplusWork;

        public SolarCal(double Kwp, double sunnyTime, double ammountMonney, double sellforEVN)
        {
            this.Kwp = Kwp;
            this.sunnyTime = sunnyTime;
            this.ammountMonney = ammountMonney;
            this.sellforEVN = sellforEVN;
            Loaded();
        }

        public double Kwh_up_Month => 30 * sunnyTime * Kwp;

        public void Loaded()
        {
        }

        public void savedMoney(NormalConsume normalConsume)
        {
            double surplus;
            var KwM = Kwh_up_Month;
            if (normalConsume.consumeMonth != 0)
                for (double i = normalConsume.rankElectricWorkList.Count; i >= 1; i--)
                {
                    var E = normalConsume.rankElectricWorkList[i];
                    E.UsedWork = E.usedPrice / E.Price;
                    surplus = KwM - E.UsedWork;
                    E.SavedWork = surplus >= 0 ? E.UsedWork : KwM;
                    KwM = surplus;
                    rankElectricWorkPrivate.Add(i, E);



                }
            else
            {
                var keyList = new string[3] { "Cao điểm", "Bình thường", "Thấp điểm" };
                foreach (var key in keyList)
                {
                    var E = normalConsume.rankElectricWorkList[key];
                    surplus = KwM - E.UsedWork;
                    E.SavedWork = surplus >= 0 ? E.UsedWork : KwM;
                    KwM = surplus;
                    rankElectricWorkPrivate.Add(key, E);
                }
            }

            SurplusWork = KwM >= 0 ? KwM : 0;
            SurplusPrice = SurplusWork * sellforEVN;
        }
        public double TongDoanhThu = 0;
        public int soNam;
        public SortedList<object, DoanhThuStruct> doanhthuList;
        public double phantramtanggia;
        public double suygiamcongsuat1;
        public double suygiamcongsuat;
        public void DoanhThu(int soNam, double phantramtanggia, double suygiamcongsuat1, double suygiamcongsuat)
        {
            this.soNam = soNam;
            this.phantramtanggia = phantramtanggia;
            this.suygiamcongsuat = suygiamcongsuat;
            this.suygiamcongsuat1 = suygiamcongsuat1;
            double cache = 0;
            double cache1 = 0;
            doanhthuList = new SortedList<object, DoanhThuStruct>();
            for (var i = 1; i <= soNam; i++)
            {
                var doanhThuStruct = new DoanhThuStruct();
                if (i == 1)
                {
                    doanhThuStruct.SanLuong = Kwh_up_Month * 12 * (1 - suygiamcongsuat1 / 100);
                    cache = doanhThuStruct.SanLuong;
                    doanhThuStruct.DoanhThu = doanhThuStruct.SanLuong * sellforEVN;
                    cache1 = sellforEVN;
                    doanhthuList.Add(i, doanhThuStruct);
                }
                else
                {
                    cache1 = cache1 * (1 + phantramtanggia / 100);
                    doanhThuStruct.SanLuong = cache * (1 - suygiamcongsuat / 100);
                    cache = doanhThuStruct.SanLuong;
                    doanhThuStruct.DoanhThu = doanhThuStruct.SanLuong * cache1;
                    doanhthuList.Add(i, doanhThuStruct);
                }
            }


            foreach (var o in doanhthuList) TongDoanhThu += o.Value.DoanhThu;

        }

        public struct DoanhThuStruct
        {
            public double SanLuong;
            public double DoanhThu;
        }
    }
}
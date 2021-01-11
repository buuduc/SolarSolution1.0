using System;
using System.Collections.Generic;
using System.Globalization;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;

namespace SolarSolution
{
    public partial class ReportDE : XtraReport
    {
        public ReportDE()
        {
            InitializeComponent();
        }
        CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
        public NormalConsume NormalConsume { get; set; }
        public SolarCal SolarCal { get; set; }

        public string tenkhachhang
        {
            set => tenkhachhanglb.Text = $"{value}";
        }

        public string diachi
        {
            set => diachilb.Text = $"{value}";
        }

        public string dienkinhdoanh
        {
            set => dienkinhdoanhlb.Text = $"{value}";
        }

        public void InitData()
        {
           
            congSuattxt.Text = $"{SolarCal.Kwp.ToString("0,0", elGR)} Kwp";
            kinhphitxt.Text = $"{SolarCal.ammountMonney.ToString("0,0", elGR)} VNĐ";
            soGionangtxt.Text = $"{SolarCal.sunnyTime} giờ/ngày";
            tuoithotxt.Text = $"{SolarCal.soNam} năm";
            giabantxt.Text= $"{SolarCal.sellforEVN.ToString("0,0", elGR)} VNĐ";
            tanggiatxt.Text= $"{SolarCal.phantramtanggia.ToString(",0", elGR)} % mỗi năm";
            suygiam1txt.Text= $"{SolarCal.suygiamcongsuat1.ToString(",0", elGR)} % ";
            suygiamtxt.Text = $"{SolarCal.suygiamcongsuat.ToString(",0", elGR)} % ";
            setRanktable();
            setPricePerRankTable();
            TableWattagePerYears();
            TongDoanhThu();
            setPieChart();
            // tenkhachhanglb.Text = tenkhachhang;
            // diachilb.Text = diachi;
            // dienkinhdoanhlb.Text = dienkinhdoanh;
        }
        public void setPieChart()
        {
           
            Series series1 = new Series("2018", ViewType.Bar);
            series1.ArgumentDataMember = "Region";
            series1.ValueDataMembers.AddRange("Value1");

            // Create the second series and specify its data members.
            Series series2 = new Series("2019", ViewType.Bar);
            series2.ArgumentDataMember = "Region";
            series2.ValueDataMembers.AddRange("Value2");
        }
        private void setPricePerRankTable()
        {
            var headTableRow = new XRTableRow();
            PricePerRanktb.Rows.Clear();
            switch (NormalConsume.consumeMonth)
            {
                default:
                    {
                        var list = new List<string> { "Bậc", "Kinh phí (VNĐ)" };
                        foreach (var VARIABLE in list)
                        {
                            var cell = new XRTableCell() { Text = VARIABLE };
                            headTableRow.Cells.Add(cell);
                        }
                        PricePerRanktb.Rows.Add(headTableRow);
                        foreach (var k in SolarCal.rankElectricWorkPrivate)
                        {
                            var row = new XRTableRow();
                            var cell1 = new XRTableCell();
                            cell1.Text = $"{k.Key}";
                            row.Cells.Add(cell1);
                            var cell2 = new XRTableCell();
                            cell2.Text = $"{k.Value.usedPrice.ToString("0,0", elGR)}";
                            row.Cells.Add(cell2);
                            PricePerRanktb.Rows.Add(row);
                        }



                        break;
                    }
                case 0:
                    {
                        var list = new List<string> { "Thời điểm", "Kinh phí (VNĐ)" };
                        foreach (var VARIABLE in list)
                        {
                            var cell = new XRTableCell() { Text = VARIABLE };
                            headTableRow.Cells.Add(cell);
                        }

                        PricePerRanktb.Rows.Add(headTableRow);
                        foreach (var k in SolarCal.rankElectricWorkPrivate)
                        {
                            var row = new XRTableRow();
                            var cell1 = new XRTableCell();
                            cell1.Text = $"{k.Key}";
                            row.Cells.Add(cell1);
                            var cell2 = new XRTableCell();
                            cell2.Text = $"{k.Value.usedPrice.ToString("0,0", elGR)}";
                            row.Cells.Add(cell2);

                            PricePerRanktb.Rows.Add(row);
                        }
                        break;
                    }
            }
        }
        public void setRanktable()
        {
            var headTableRow = new XRTableRow();
            rankTable.Rows.Clear();
            switch (NormalConsume.consumeMonth)
            {
                default:
                    {
                        tienDientxt.Text = $"{NormalConsume.consumeMonth.ToString("0,0", elGR)} VNĐ";
                        var list = new List<string> { "Bậc", "Công suất giới hạn", "Đơn giá (VNĐ)" };
                        foreach (var VARIABLE in list)
                        {
                            var cell = new XRTableCell() { Text = VARIABLE };
                            headTableRow.Cells.Add(cell);
                        }
                        rankTable.Rows.Add(headTableRow);
                        foreach (var k in SolarCal.rankElectricWorkPrivate)
                        {
                            var row = new XRTableRow();
                            var cell1 = new XRTableCell();
                            cell1.Text = $"{k.Key}";
                            row.Cells.Add(cell1);
                            var cell2 = new XRTableCell();
                            cell2.Text = $"{k.Value.quantityAllowed.ToString("0,0", elGR)}";
                            row.Cells.Add(cell2);
                            var cell3 = new XRTableCell();
                            cell3.Text = $"{k.Value.Price.ToString("0,0", elGR)}";
                            row.Cells.Add(cell3);
                            rankTable.Rows.Add(row);
                        }



                        break;
                    }
                case 0:
                    {
                        tienDientxt.Text = $"{NormalConsume.AmmountMoneyforEnterprise.ToString("0,0", elGR)} VNĐ";
                        var list = new List<string> { "Thời điểm", "Đơn giá (VNĐ)" };
                        foreach (var VARIABLE in list)
                        {
                            var cell = new XRTableCell() { Text = VARIABLE };
                            headTableRow.Cells.Add(cell);
                        }

                        rankTable.Rows.Add(headTableRow);
                        foreach (var k in SolarCal.rankElectricWorkPrivate)
                        {
                            var row = new XRTableRow();
                            var cell1 = new XRTableCell();
                            cell1.Text = $"{k.Key}";
                            row.Cells.Add(cell1);
                            var cell2 = new XRTableCell();
                            cell2.Text = $"{k.Value.Price.ToString("0,0", elGR)}";
                            row.Cells.Add(cell2);

                            rankTable.Rows.Add(row);
                        }
                        break;
                    }
            }

            //foreach (var k in SolarCal.rankElectricWorkPrivate)
            //{
            //    var row = new XRTableRow();
            //    var cell1 = new XRTableCell();
            //    cell1.Text = $"{k.Key}";
            //    row.Cells.Add(cell1);
            //    var cell2 = new XRTableCell();

            //    cell2.Text = $"{k.Value.UsedWork}";
            //    row.Cells.Add(cell2);
            //    xrTable1.Rows.Add(row);
            //}
        }

        void TongDoanhThu()
        {
            TongDoanhThutxt.Text =
                $"Như vậy với tuổi thọ của pin năng lượng mặt trời này, thì doanh thu nhận được sau {SolarCal.soNam.ToString("0,0", elGR)} năm xấp xỉ {SolarCal.TongDoanhThu.ToString("0,0", elGR)} VNĐ.";
        }
        public void TableWattagePerYears()
        {

            //xrTable1.BeginInit();
            xrTable1.Rows.Clear();
            var headTableRow = new XRTableRow();
            var list = new List<string> { "Năm", "Sản lượng", "Doanh thu" };
            foreach (var VARIABLE in list)
            {
                var cell = new XRTableCell() { Text = VARIABLE };
                headTableRow.Cells.Add(cell);
            }
            xrTable1.Rows.Add(headTableRow);
            foreach (var k in SolarCal.doanhthuList)
            {
                var row = new XRTableRow();

                var cell1 = new XRTableCell();
                cell1.Text = $"{k.Key}";
                row.Cells.Add(cell1);

                var cell2 = new XRTableCell();
                cell2.Text = $"{Math.Round(k.Value.SanLuong, 0).ToString("0,0", elGR)}";
                row.Cells.Add(cell2);


                var cell3 = new XRTableCell();
                cell3.Text = $"{Math.Round(k.Value.DoanhThu, 0).ToString("0,0", elGR)}";
                row.Cells.Add(cell3);

                xrTable1.Rows.Add(row);
            }
            //W_Per_Years_Table.EndInit();


            //xrTableCell4.Text = "fggg";
            //xrTableCell5.Text = "fggg";
            //xrTable1.EndInit();
        }
    }
}
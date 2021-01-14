using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OfficeOpenXml;
using System.IO;
using System.Text.RegularExpressions;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Printing;

using DevExpress.XtraReports.UI;



namespace SolarSolution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Hashtable soGioNangHashtable = new Hashtable(); // đọc data số giờ nắng từ file database
        private NormalConsume normalConsume;// đối tượng thể hiện số tiền chi trả 
        private SolarCal solarCal;
        private SortedList<object, rankElectricWork> rankE;
        private SortedList<int, TabItem> tablist = new SortedList<int, TabItem>();
        public string DienKinhDoanh;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            
        }
        
        private void Loaded_Windows(object sender, RoutedEventArgs e)
        {


            
            hideTab();

            //((System.Windows.UIElement)TabControlGeneral.Items[1]).IsVisible = Visibility.Hidden;




        }
        private void hideTab()
        {
            tablist.Add(1, tab1);
            tablist.Add(2, tab2);
            tablist.Add(3, tab3);
            tablist.Add(4, tab4);
            tablist.Add(5, tab5);
            foreach (TabItem item in tablist.Values)
            {
                item.Visibility = Visibility.Collapsed;
            }

        }
        public struct rankElectricWork
        {
            public double Price;
            public double quantityAllowed;
            public double usedPrice;

            public double UsedWork;
            public double SavedWork;
            public double SavedPrice => SavedWork * Price;

        }

        private void ReadDataExcel(int index)
        {
            try
            {
                rankE = null;
                soGioNangHashtable.Clear();
                rankE = new SortedList<object, rankElectricWork>();

                string path = @Properties.Settings.Default.pathDataExcel;
                using (ExcelPackage MaNS =
                    new ExcelPackage(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                {
                    ExcelWorksheet workSheet = MaNS.Workbook.Worksheets[0];

                    for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                    {
                        soGioNangHashtable.Add(workSheet.Cells[i, 2].Value, workSheet.Cells[i, 3].Value);
                    }


                    workSheet = MaNS.Workbook.Worksheets[index];
                    switch (index)
                    {
                        case 1:
                            {
                                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                                {
                                    rankElectricWork E = new rankElectricWork();
                                    E.Price = (double)workSheet.Cells[i, 2].Value;
                                    E.quantityAllowed = (double)workSheet.Cells[i, 3].Value;
                                    rankE.Add(workSheet.Cells[i, 1].Value, E);

                                    // rankElectricWorkPrice.Add(, );
                                    // rankElectricWorkCount.Add(workSheet.Cells[i, 1].Value, );
                                    normalConsume = new NormalConsume()
                                    {
                                        rankElectricWorkList = rankE
                                    };
                                }
                                break;

                            }
                        default:
                            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                            {
                                rankElectricWork E = new rankElectricWork();
                                E.Price = (double)workSheet.Cells[i, 2].Value;
                                rankE.Add(workSheet.Cells[i, 1].Value, E);
                                // rankElectricWorkPrice.Add(, );
                                // rankElectricWorkCount.Add(workSheet.Cells[i, 1].Value, );
                                normalConsume = new NormalConsume()
                                {
                                    rankElectricWorkList = rankE
                                };
                            }
                            break;


                    }
                    // normalConsume = new NormalConsume(700, 3100, 1000)
                    // normalConsume = new NormalConsume(5000000)
                    // {
                    //     rankElectricWorkList = rankE
                    // };



                }

                khuvucComboBox.ItemsSource = soGioNangHashtable.Keys;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;

                
            }
            

        }
        private void pullData()
        {
            switch (DienKinhDoanh)
            {
                case "Hộ gia đình":
                    {
                        normalConsume.consumeMonth = Double.Parse(thapdiemtxt.Text);
                        break;
                    }
                default:
                    { 
                        normalConsume.Acaodiem= Double.Parse(caodiemtxt.Text);
                        normalConsume.Atrungbinh = Double.Parse(trungbinhtxt.Text);
                        normalConsume.Athapdiem= Double.Parse(thapdiemtxt.Text);
                        break; 
                    }

            }
        }
        private void ExistBtn(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ThicknessConverter tc = new ThicknessConverter();
            var thin = (Thickness)tc.ConvertFromString("0.5");
            RadioButton radioButton = sender as RadioButton;
            DienKinhDoanh = radioButton.Content.ToString();
            switch (radioButton.Content)
            {
                case "Hộ gia đình":
                    {
                        HideShowGroupControl(false);
                        ReadDataExcel(1);
                        RankTable.RowGroups.Clear();

                        TableRow row = new TableRow();
                        row.Cells.Add(new TableCell(new Paragraph(new Run("Bậc"))) { BorderBrush = Brushes.Black, BorderThickness=thin });
                        row.Cells.Add(new TableCell(new Paragraph(new Run("Giới hạn"))) { BorderBrush = Brushes.Black, BorderThickness = thin });
                        row.Cells.Add(new TableCell(new Paragraph(new Run("Đơn giá"))) { BorderBrush = Brushes.Black, BorderThickness = thin });
                        RankTable.RowGroups.Add(new TableRowGroup());
                        RankTable.RowGroups[0].Rows.Add(row);
                        foreach (var itemElectricWork in rankE)
                        {
                            row = new TableRow();
                            row.Cells.Add(new TableCell(new Paragraph(new Run(itemElectricWork.Key.ToString()))) { BorderBrush = Brushes.Black, BorderThickness = thin });
                            row.Cells.Add(new TableCell(new Paragraph(new Run(itemElectricWork.Value.quantityAllowed.ToString()))) { BorderBrush = Brushes.Black, BorderThickness = thin });
                            row.Cells.Add(new TableCell(new Paragraph(new Run(itemElectricWork.Value.Price.ToString()))) { BorderBrush = Brushes.Black, BorderThickness = thin });
                            RankTable.RowGroups.Add(new TableRowGroup());
                            RankTable.RowGroups[RankTable.RowGroups.Count - 1].Rows.Add(row);



                        }
                        break;
                    }
                case "Diện kinh doanh":
                    {
                        HideShowGroupControl(true);
                        ReadDataExcel(2);
                        RankTable.RowGroups.Clear();

                        TableRow row = new TableRow();
                        row.Cells.Add(new TableCell(new Paragraph(new Run("Thời điểm"))) { BorderBrush = Brushes.Black, BorderThickness = thin });
                        row.Cells.Add(new TableCell(new Paragraph(new Run("Đơn giá"))) { BorderBrush = Brushes.Black, BorderThickness = thin });
                        RankTable.RowGroups.Add(new TableRowGroup());
                        RankTable.RowGroups[0].Rows.Add(row);
                        foreach (var itemElectricWork in rankE)
                        {
                            row = new TableRow();
                            row.Cells.Add(new TableCell(new Paragraph(new Run(itemElectricWork.Key.ToString()))) { BorderBrush = Brushes.Black, BorderThickness = thin });
                            row.Cells.Add(new TableCell(new Paragraph(new Run(itemElectricWork.Value.Price.ToString()))) { BorderBrush = Brushes.Black, BorderThickness = thin });
                            RankTable.RowGroups.Add(new TableRowGroup());
                            RankTable.RowGroups[RankTable.RowGroups.Count - 1].Rows.Add(row);



                        }
                        break;
                    }
                case "Diện sản xuất":
                    {
                        HideShowGroupControl(true);
                        ReadDataExcel(3);
                        RankTable.RowGroups.Clear();

                        TableRow row = new TableRow();
                        row.Cells.Add(new TableCell(new Paragraph(new Run("Thời điểm"))) { BorderBrush = Brushes.Black, BorderThickness = thin });
                        row.Cells.Add(new TableCell(new Paragraph(new Run("Đơn giá"))) { BorderBrush = Brushes.Black, BorderThickness = thin });
                        RankTable.RowGroups.Add(new TableRowGroup());
                        RankTable.RowGroups[0].Rows.Add(row);
                        foreach (var itemElectricWork in rankE)
                        {
                            row = new TableRow();
                            row.Cells.Add(new TableCell(new Paragraph(new Run(itemElectricWork.Key.ToString()))) { BorderBrush = Brushes.Black, BorderThickness = thin });
                            row.Cells.Add(new TableCell(new Paragraph(new Run(itemElectricWork.Value.Price.ToString()))) { BorderBrush = Brushes.Black, BorderThickness = thin });
                            RankTable.RowGroups.Add(new TableRowGroup());
                            RankTable.RowGroups[RankTable.RowGroups.Count - 1].Rows.Add(row);



                        }
                        break;
                    }
            }

        }


        private void PreviousBtn_clicked(object sender, RoutedEventArgs e)
        {
            TabControlGeneral.SelectedIndex -= 1;
        }

        private void NextBtn_clicked(object sender, RoutedEventArgs e)
        {
            if (TabControlGeneral.SelectedIndex <TabControlGeneral.Items.Count-1)
            {
                tablist.Values[TabControlGeneral.SelectedIndex ].Visibility = Visibility.Visible;
            }
            
            TabControlGeneral.SelectedIndex += 1;
            
        }

        private void TabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int maxtab = TabControlGeneral.Items.Count;
            int curtab = TabControlGeneral.SelectedIndex;
            if (curtab == 0)
            {
                PreviousBtn.IsEnabled = false;
            }
            else if (curtab == maxtab - 1)
            {
                NextBtn.IsEnabled = false;
            }
            else
            {
                PreviousBtn.IsEnabled = true;
                NextBtn.IsEnabled = true;
            }
        }

        private void khuvuctxtbox_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            SogioNangTxt.Text = soGioNangHashtable[comboBox.SelectedItem].ToString();

        }

        public string testthu = "ggxx";

        

        private void XemBaoCaoBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pullData();




                normalConsume.Loaded();
                ReportDE reportDe = new ReportDE();


                //solarCal = new SolarCal(50, 4.1, 800000000, 1969);
                solarCal = new SolarCal(double.Parse(SoKWpLapDatTextBox.Text), double.Parse(SogioNangTxt.Text), double.Parse(kinhphitxt.Text), double.Parse(giabantxt.Text));
                solarCal.savedMoney(normalConsume);
                solarCal.DoanhThu(Int32.Parse(tuoithotxt.Text), double.Parse(tanggiatxt.Text), double.Parse(suygiamcongsuat1txt.Text), double.Parse(suygiamcongsuattxt.Text));
                reportDe.tenkhachhang = TenKhachHangtxt.Text;
                reportDe.diachi = DiaChiTxt.Text;
                reportDe.dienkinhdoanh = DienKinhDoanh;
                reportDe.NormalConsume = normalConsume;

                reportDe.SolarCal = solarCal;
                reportDe.InitData();
                reportDe.CreateDocument();
                //DocumentPreviewControl.DocumentSource = reportDe;



                reportDe.ShowPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
                throw;
            }
            


        }
        private void HideShowGroupControl(bool c)
        {
            switch (c)
            {
                case true:
                    {
                        thapdiemlb.Text = "Thấp điểm:";
                        VNDtxt1.Text = "KWh";
                        
                        trungbinhlb.Visibility = Visibility.Visible;
                        Caodiemlb.Visibility = Visibility.Visible;
                        trungbinhtxt.Visibility = Visibility.Visible;
                        caodiemtxt.Visibility = Visibility.Visible;
                        VNDtxt2.Visibility = Visibility.Visible;
                        VNDtxt3.Visibility = Visibility.Visible;


                        break;
                    }
                case false:
                    {
                        thapdiemlb.Text = "Số tiền:";
                        VNDtxt1.Text = "VNĐ";
                        trungbinhlb.Visibility = Visibility.Hidden;
                        Caodiemlb.Visibility = Visibility.Hidden;
                        trungbinhtxt.Visibility = Visibility.Hidden;
                        caodiemtxt.Visibility = Visibility.Hidden;
                        VNDtxt2.Visibility = Visibility.Hidden;
                        VNDtxt3.Visibility = Visibility.Hidden;
                        break;
                    }
            }
                
        }
        private void ContactBtn(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://marshouse.vn/contact/");
        }

        private void marshousebtn(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://marshouse.vn/");
        }

        private void databtn(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@Properties.Settings.Default.pathDataExcel);
        }

        
    }

}

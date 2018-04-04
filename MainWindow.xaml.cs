using System;
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
using System.Windows.Forms;
using System.IO;

namespace FaZhi_Downloader
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DateTime fromDate;
        private DateTime toDate;
        private bool confirmDate = false;
        private string path = null;
        public MainWindow()
        {
            InitializeComponent();
            
            this.Closing += Window_Closing; //添加关闭窗口事件函数
            CalendarDateRange cdrFrom = new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue);
            this.fromDate_cal.BlackoutDates.Add(cdrFrom);
            this.fromDate_cal.SelectedDate = DateTime.Today;
            CalendarDateRange cdrTo = new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue);
            this.toDate_cal.BlackoutDates.Add(cdrTo);
            this.toDate_cal.SelectedDate = DateTime.Today;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string saveDir = System.Environment.CurrentDirectory + "\\log";
            string stmpFile = saveDir + "\\temp.txt";
            if (File.Exists(stmpFile))
            {
                File.Delete(stmpFile);
            }
            if (Directory.Exists(saveDir))
            { 
                Directory.Delete(saveDir);
            }
        }
        //选择保存路径按钮
        private void choose_btn_Click(object sender, RoutedEventArgs e)
        {
            path = string.Empty;
            var fbd = new FolderBrowserDialog(); //目录选择
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = fbd.SelectedPath;
            }
            this.location_txt.Content = path;
        }
        //确定时间段
        private void confirm_btn_Click(object sender, RoutedEventArgs e)
        {
            confirmDate = true;
            fromDate = (DateTime)this.fromDate_cal.SelectedDate;
            toDate = (DateTime)this.toDate_cal.SelectedDate;
            {
                this.chooseAns_txt.Content =
                    "您选择的日期是：从" +
                    fromDate.ToString("d") + "到" +
                    toDate.ToString("d");
            }

        }
        //下载确定按钮
        private void downLoad_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!confirmDate)
            {
                System.Windows.MessageBox.Show("请先确定时间！");
                return;
            }
            Downloader downloader = new Downloader(path,fromDate, toDate);
            downloader.startDownload();
        }



    }
}

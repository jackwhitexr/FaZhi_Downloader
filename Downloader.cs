using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace FaZhi_Downloader
{
    /**
     * 根据固定规则链接地址生成downloadLink.txt
     * 调用aria2下载
     * 重定向显示
     */
    class Downloader
    {
        private DateTime fromDate;
        private DateTime toDate;
        private string path;
        private string defaultLink =
            "http://live.njgb.com/NJRadio/FM106.9/*/fl3659/fl3659_$_143000-150000.mp3"; //固定下载链接
        private  ArrayList dateRange = null;
        private static string saveDir = System.Environment.CurrentDirectory + "\\log";
        private static string stmpFile = saveDir + "\\temp.txt";
        public Downloader(string path,DateTime fromDate, DateTime toDate)
        {
            this.path = path;
            this.fromDate = fromDate;
            this.toDate = toDate;
        }
        private void getDate()
        {
            dateRange = new ArrayList();
            dateRange.Add(fromDate.ToString("yyyy-MM-dd")); //月份是大写的MM
            DateTime tmpDate = fromDate;
            while (tmpDate != toDate)
            {
                tmpDate = tmpDate.AddDays(1);
                dateRange.Add(tmpDate.ToString("yyyy-MM-dd"));
            }
        }   
        private void getDownloadLink(DateTime fromDate, DateTime toDate)
        {
            
            if (File.Exists(stmpFile))
            {
                File.Delete(stmpFile);
            }
            if (Directory.Exists(saveDir))
            {
                Directory.Delete(saveDir);
            }
            Directory.CreateDirectory(saveDir);
            //File.Create(stmpFile);
            getDate();
            using (StreamWriter sw = File.CreateText(stmpFile))
            {
                for (int i = 0; i < dateRange.Count; i++)
                {
                    string tmp = (string)dateRange[i]; tmp=tmp.Replace("/", "-");
                    string tmp2 = tmp.Replace("-", "");
                    string link1 =defaultLink.Replace("*",tmp);
                    string link2 = link1.Replace("$", tmp2);
                    sw.WriteLine(link2);
                }
            }

        }
        public void startDownload()
        {
            getDownloadLink(fromDate, toDate);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Process p = new Process();
            p.StartInfo.FileName = System.Environment.CurrentDirectory + "\\aria2\\aria2c.exe";
            p.StartInfo.Arguments = " -i " + stmpFile+"  -d "+path+"\\download";
            p.Start();
            p.WaitForExit();
        }

    }
}

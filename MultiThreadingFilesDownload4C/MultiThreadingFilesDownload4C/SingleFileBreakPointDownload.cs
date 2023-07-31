#define GET_URL_SIZE 
//#define GET_URLS_SIZE 

using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LearningDemo
{
    public partial class SingleFileBreakPointDownload : DevExpress.XtraEditors.XtraForm
    {
        public SingleFileBreakPointDownload()
        {
            InitializeComponent();
        }

        static bool isPause = true;
        static long rangeBegin = 0;
        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            isPause = !isPause;
            if (!isPause)//点击下载
            {
                button1.Text = "暂停";

                await Task.Run(async () =>
                {
                    //异步操作UI元素
                    label1.Invoke((Action)(() =>
                    {
                        label1.Text = "准备下载...";
                    }));

                    long downloadSpeed = 0;//下载速度
                    using (HttpClient http = new HttpClient())
                    {
                        var url1 = @"https://dldir1.qq.com/qqfile/qq/PCQQ9.7.1/QQ9.7.1.28940.exe";
                        var url2 = @"http://stossbackup.libooc.com/masteroffice.libooc.com/notepad/big_img.jpeg";
                        var url3 = @"https://webcdn.m.qq.com/spcmgr/download/QQMusic_Setup_1942.2219_QMgr.exe";
                        HashSet<string> urls = new HashSet<string>();
                        urls.AddRange(new string[] { url1, url2, url3 });

                        #if GET_URL_SIZE
                        var request = new HttpRequestMessage { RequestUri = new Uri(url2) };
                        request.Headers.Range = new RangeHeaderValue(rangeBegin, null); //【关键点】全局变量记录已经下载了多少，然后下次从这个位置开始下载。
                        var httpResponseMessage = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                        var contentLength = httpResponseMessage.Content.Headers.ContentLength;//本次请求的内容大小  
                        #endif

                        #if GET_URLS_SIZE
                        HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                        long totalFilesSize = 0L;
                        foreach ( var url in urls)
                        {
                            var request = new HttpRequestMessage { RequestUri = new Uri(url) };
                            request.Headers.Range = new RangeHeaderValue(rangeBegin, null); //【关键点】全局变量记录已经下载了多少，然后下次从这个位置开始下载。
                            httpResponseMessage = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                            var contentLength = httpResponseMessage.Content.Headers.ContentLength;//本次请求的内容大小  
                            if (httpResponseMessage.Content.Headers.ContentRange != null) //如果为空，则说明服务器不支持断点续传
                            {
                                contentLength = httpResponseMessage.Content.Headers.ContentRange.Length;//服务器上的文件大小
                                totalFilesSize += contentLength.Value;
                            }
                        }
                        #endif

                        

                        using (var stream = await httpResponseMessage.Content.ReadAsStreamAsync())
                        {
                            var readLength = 1024*1024*5;//5MB
                            byte[] bytes = new byte[readLength];
                            int writeLength;
                            var beginSecond = DateTime.Now.Second;//当前时间秒
                            var target = @"D:\Zane\Test\Download\QQ9.7.1.28940.exe";
                            var target2 = @"D:\Zane\Test\Download\big_img.jpeg";
                            while ((writeLength = stream.Read(bytes, 0, readLength)) > 0 && !isPause)
                            {
                                try
                                {
                                    //使用追加方式打开一个文件流
                                    using (FileStream fs = new FileStream(target2, FileMode.Append, FileAccess.Write))//Application.StartupPath + "/QQ9.7.1.28940.exe"
                                    {
                                        fs.Write(bytes, 0, writeLength);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    await Console.Out.WriteLineAsync( $"file write error: {e}"); ;
                                }
                                downloadSpeed += writeLength;
                                rangeBegin += writeLength;
                                progressBar1.Invoke((Action)(() =>
                                {
                                    var endSecond = DateTime.Now.Second;
                                    if (beginSecond != endSecond)//计算速度
                                    {
                                        downloadSpeed = downloadSpeed / (endSecond - beginSecond);
                                        string speedSize = "KB/S";
                                        int downloadSpeedBase = 1024;
                                        if (contentLength > (1024*1024*100))
                                        {
                                            speedSize = "MB/S";
                                            downloadSpeedBase = 1024 * 1024;
                                        }
                                        label1.Text = "下载速度" + downloadSpeed / downloadSpeedBase + $"{speedSize}";

                                        beginSecond = DateTime.Now.Second;
                                        downloadSpeed = 0;//清空
                                    }
                                    progressBar1.EditValue = Math.Max((int)((rangeBegin) * 100 / contentLength), 1);
                                }));
                            }

                            if (rangeBegin == contentLength)
                            {
                                label1.Invoke((Action)(() =>
                                {
                                    label1.Text = "下载完成";
                                }));
                            }
                        }
                    }
                });
            }
            else//点击暂停
            {
                button1.Text = "继续下载";
                label1.Text = "暂停下载";
            }
        }

    }
}
#define GET_URL_SIZE
using DevExpress.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LearningDemo
{
    public partial class MultpleFilesBreakPointDownload : Form
    {
        public MultpleFilesBreakPointDownload()
        {
            InitializeComponent();
        }

        static bool isPause = true;
        static long rangeBegin = 0;
        static Dictionary<string, long> ecuByte = new Dictionary<string, long>();// for store per ecu bytes
        static Dictionary<string, ProgressBar> progressBars = null;
        static HttpClient httpClient = new HttpClient();
        private async void button1_Click(object sender, EventArgs e)
        {
            #region Data Initial
            Dictionary<string, List<string>> ecus = new Dictionary<string, List<string>>()
            {
                { "ecu1",
                    new List<string>{ "https://cdn.apifox.cn/download/Apifox-windows-latest.zip", "https://dldir1.qq.com/music/clntupate/QQMusic_YQQWinPCDL.exe" } },
                { "ecu2",
                    new List<string>{ "https://dldir1.qq.com/weixin/Windows/WeChatSetup.exe", "https://dldir1.qq.com/qqfile/qq/PCQQ9.7.12/QQ9.7.12.29112.exe" } },
            };

            #endregion
            isPause = !isPause;
            if (!isPause)//点击下载
            {
                button1.Text = "暂停";

                #region Get All ECU Bytes and All Files Bytes
                long totalByteSize = 0; // for store all ecu bytes
                // get all bytes of urls
                await Task.Run(async () =>
                {
                    // get all per ecu byte sizes
                    foreach (var kvp in ecus)
                    {
                        string ecuKey = kvp.Key;
                        List<string> urls = kvp.Value;
                        long ecuByteSize = 0;// record ecu bytes <fixed location>
                        foreach (var url in urls)
                        {
                            //using (HttpClient httpClient = new HttpClient())
                            //{
                            var httpResponseMessage = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                            var contentLength = httpResponseMessage.Content.Headers.ContentLength;
                            if (contentLength == 0)
                            {
                                throw new Exception("Content length is 0.");
                            }
                            totalByteSize += contentLength ?? 0;
                            ecuByteSize += contentLength ?? 0;
                            //}
                        }
                        ecuByte.Add(ecuKey, ecuByteSize);// per ecu size
                    }
                });
                long AllFilesByteSizes = totalByteSize; // get all of bytes size of urls
                #endregion

                // Create a progress bar for each file
                progressBars = new Dictionary<string, ProgressBar>()
                {
                    { "ecu1", progressBar2},
                    { "ecu2", progressBar3},
                };

                await Task.Run(async () =>
                {
                    //异步操作UI元素
                    label1.Invoke((Action)(() =>
                    {
                        label1.Text = "准备下载...";
                    }));

                    foreach (var kvp in ecus)
                    {
                        string key = kvp.Key;
                        List<string> urls = kvp.Value;

                        // Create a folder based on the key value
                        string folderPath = $@"D:\Zane\Test\Download\{key}";
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        else
                        {
                            DirectoryInfo directory = new DirectoryInfo(folderPath);
                            long floderRangeBegin = 0;
                            if (directory.GetFiles().Count() > 0)
                            {
                                Array.ForEach(directory.GetFiles(), (file) => { floderRangeBegin += file.Length; });

                                if (floderRangeBegin >= ecuByte[key])
                                {
                                    progressBars[key].Invoke((Action)(() =>
                                    {
                                        progressBars[key].Value = Math.Max((int)((floderRangeBegin * 100) / ecuByte[key]), 100);
                                    }));
                                    continue;
                                }
                                else
                                {
                                    progressBars[key].Invoke((Action)(() =>
                                    {
                                        progressBars[key].Value = Math.Max((int)((floderRangeBegin * 100) / ecuByte[key]), 1);
                                    }));
                                }
                            }
                        }

                        try
                        {
                            // Start parallel tasks to download the files
                            await Task.WhenAll(urls.Select(url => DownloadFile(url, folderPath, key)));//, progressBars[key]

                            progressBars[key].Invoke((Action)(() =>
                            {
                                progressBars[key].Value = Math.Max((int)((rangeBegin * 100) / ecuByte[key]), 100);
                            }));
                            label1.Invoke((Action)(() =>
                            {
                                label1.Text = $"{key}下载完成";
                            }));
                        }
                        catch (Exception ex)
                        {
                            await Console.Out.WriteLineAsync($"{ex}");
                        }
                    }
                    label1.Invoke((Action)(() =>
                    {
                        label1.Text = "全部下载完成";
                    }));
                });
            }
            else//点击暂停
            {
                button1.Text = "继续下载";
                label1.Text = "暂停下载";
            }
        }

        private async Task DownloadFile(string url, string folderPath, string key)//, ProgressBar progressBar
        {
            string fileName = GetUrlFileName(url);
            string filePath = Path.Combine(folderPath, fileName);
            FileInfo fileInfo = new FileInfo(filePath);
            if (File.Exists(filePath))
            {
                rangeBegin = fileInfo.Length;
                progressBars[key].Invoke((Action)(() =>
                {
                    progressBars[key].Value = Math.Max((int)((rangeBegin * 100) / ecuByte[key]), 1);
                }));
            }
            else
            {
                rangeBegin = 0;
            }
            //using (HttpClient httpClient = new HttpClient())
            //{
            //var request = new HttpRequestMessage() { RequestUri = new Uri(url),Method = HttpMethod.Get };
            //request.Headers.Range = new RangeHeaderValue(rangeBegin, null);
            httpClient.DefaultRequestHeaders.Range = new RangeHeaderValue(rangeBegin, null);
            var httpResponseMessage = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            var contentLength = httpResponseMessage.Content.Headers.ContentLength;
            if (contentLength == 0 || contentLength == null)
            {
                // The url already has been read completely
                rangeBegin = fileInfo.Length;
                progressBars[key].Invoke((Action)(() =>
                {
                    progressBars[key].Value = Math.Max((int)((rangeBegin * 100) / ecuByte[key]), 1);
                }));
                return;
            }


            int writeLength = 0; //already wrote bytes
            var readLength = 1024000;//1000K
            byte[] bytes = new byte[readLength];
            long singleUrlTotalBytes = rangeBegin + contentLength.Value;
            using (var stream = await httpResponseMessage.Content.ReadAsStreamAsync())
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                {
                    //await httpResponseMessage.Content.CopyToAsync(fs);
                    while ((writeLength = stream.Read(bytes, 0, readLength)) > 0 && !isPause)
                    {
                        // Update the progress bar for this file
                        progressBars[key].Invoke((Action)(() =>
                        {
                            progressBars[key].Value = Math.Max((int)((rangeBegin * 100) / ecuByte[key]), 1);
                        }));

                        fs.Write(bytes, 0, writeLength);
                        rangeBegin += writeLength;
                    }
                }
            }
            if (rangeBegin >= singleUrlTotalBytes)
            {
                progressBars[key].Invoke((Action)(() =>
                {
                    progressBars[key].Value = Math.Max((int)((rangeBegin * 100) / ecuByte[key]), 1);
                }));
            }
            //}
        }

        private static string GetUrlFileName(string url)
        {
            int fileNameLastIndex = url.LastIndexOf("/");
            string fileName;
            if (fileNameLastIndex != -1)
            {
                fileName = url.Substring(fileNameLastIndex + 1);
            }
            else
            {
                fileName = "NoItemFound";
            }
            return fileName;
        }
    }
}

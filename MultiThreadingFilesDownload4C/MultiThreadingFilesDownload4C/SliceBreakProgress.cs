//#define SINGLE_FILE_LOGIC
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using System.Security.Policy;

namespace LearningDemo
{
    public partial class SliceBreakProgress : Form
    {
        private string testBaseAddress = @"D:\Zane\Test\Download";
        public SliceBreakProgress()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            #region Test Data Init
            Dictionary<string, List<string>> ecus = new Dictionary<string, List<string>>()
                {
                    { "Ecu1", new List<string>
                    {
                        "https://dldir1.qq.com/weixin/Windows/WeChatSetup.exe",
                        "https://dldir1.qq.com/qqfile/qq/PCQQ9.7.12/QQ9.7.12.29112.exe"
                    }
        },
                    { "Ecu2", new List<string> {
                        "https://updatecdn.meeting.qq.com/cos/10335c07a804e37b63e2b17b6c880010/TencentMeeting_0300000000_3.18.5.411.publish.officialwebsite.exe",
                        "https://cdn.apifox.cn/download/Apifox-windows-latest.zip",
                    }},
                };
            List<ProgressBar> progressBar = new List<ProgressBar>() { totalBar, bar1, bar2, bar3 };
            #endregion
            
        }

        private bool MultipleFilesDownload(Dictionary<string, List<string>> downItems)
        {
            List<ProgressBar> progressBar = new List<ProgressBar>() { totalBar, bar1, bar2, bar3 };
            var tasks = new List<Task>();
            foreach (var item in downItems)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    var task1 = Task.Run(async () =>
                    {
                        using (HttpClient http = new HttpClient())
                        {
                            var httpResponseMessage = await http.GetAsync(item.Value[i], HttpCompletionOption.ResponseHeadersRead);
                            var contentLength = httpResponseMessage.Content.Headers.ContentLength.Value;
                            var size = contentLength / 10;
                            var begin = i * size;
                            var end = begin + size - 1;
                            var task = FileDownload(item.Value[i], begin, end, i);
                            tasks.Add(task);
                            await tasks[i];
                            progressBar[1].Value = i == item.Value.Count ? 100 : ((i + 1) * 10);
                        }                        
                        bool success = FileMerge(testBaseAddress + @"\File", GetFileNameFromUrl(item.Value[i]));//Application.StartupPath

                    });
                }
            } 
            return true;
        }


        public async Task DownloadFile(string url, long start, long end, int index, string filePath)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Range = new RangeHeaderValue(start, end);
                var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    var buffer = new byte[1024];
                    int bytesRead;
                    using (var fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                    {
                        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                        }
                    }
                }
            }
        }

        public void MergeFiles(string directoryPath, string finalFilePath)
        {
            var files = Directory.GetFiles(directoryPath).OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f)));

            using (var finalFileStream = new FileStream(finalFilePath, FileMode.Append, FileAccess.Write))
            {
                foreach (var file in files)
                {
                    using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        fileStream.CopyTo(finalFileStream);
                    }
                    System.IO.File.Delete(file);
                }
            }
            Directory.Delete(directoryPath);
        }

        /// <summary>
        /// 文件下载
        /// （如果你有兴趣，可以没个线程弄个进度条）
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public Task FileDownload(string url, long begin, long end, int index)
        {
            var task = Task.Run(async () =>
            {
                using (HttpClient http = new HttpClient())
                {
                    var request = new HttpRequestMessage { RequestUri = new Uri(url) };

                    request.Headers.Range = new RangeHeaderValue(begin, end);//【关键点】全局变量记录已经下载了多少，然后下次从这个位置开始下载。
                    var httpResponseMessage = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                    using (var stream = await httpResponseMessage.Content.ReadAsStreamAsync())
                    {
                        var readLength = 1024000;//1000K
                        byte[] bytes = new byte[readLength];
                        int writeLength;
                        var beginSecond = DateTime.Now.Second;//当前时间秒
                        var filePaht = testBaseAddress + "/File/";//Application.StartupPath
                        if (!Directory.Exists(filePaht))
                            Directory.CreateDirectory(filePaht);

                        try
                        {
                            while ((writeLength = stream.Read(bytes, 0, readLength)) > 0)
                            {
                                //使用追加方式打开一个文件流
                                using (FileStream fs = new FileStream(filePaht + index, FileMode.Append, FileAccess.Write))
                                {
                                    fs.Write(bytes, 0, writeLength);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            //如果出现异常则删掉这个文件
                            System.IO.File.Delete(filePaht + index);
                        }
                    }
                }
            });

            return task;
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FileMerge(string path, string fileName)
        {
            //这里排序一定要正确，转成数字后排序（字符串会按1 10 11排序，默认10比2小）
            foreach (var filePath in Directory.GetFiles(path).OrderBy(t => int.Parse(Path.GetFileNameWithoutExtension(t))))
            {
                //string testDownload = @"D:\Zane\Test\Download";
                using (FileStream fs = new FileStream(testBaseAddress + @"\" + fileName, FileMode.Append, FileAccess.Write))//Directory.GetParent(path).FullName
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);//读取文件到字节数组
                    fs.Write(bytes, 0, bytes.Length);//写入文件
                }
                System.IO.File.Delete(filePath);//删除分片
            }
            Directory.Delete(path);// 删除分片目录
            return true;
        }

        private string GetFileNameFromUrl(string url)
        {
            // 从URL中提取文件名
            // 在这里实现你的逻辑
            int index = url.LastIndexOf('/') + 1;
            return url.Substring(index);
        }
    }
}

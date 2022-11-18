using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EAPWPFDemo.EventAsynchronusPattern
{
    public class MutipleCallHtmlClient
    {
        /*
         * Napisz klasę która odczyta wszystkie pliki w danym folderze i wyśwetli listę objętości pliku w bajtach
         * Napisz klasę która dokonuje losowania 10 razy po 100000 liczb i wyświetla ich średnią wartość
         */

        private HttpClient _httpClient;
        
        private SendOrPostCallback _downloadHtmlCompltedCallback;

        private SendOrPostCallback _downloadHtmlProgressCallback;

        private delegate void DownloadHtmlCallback(AsyncOperation asyncOperation);
        
        public event EventHandler<DownloadHtmlCompletedEvntArgs> DownloadHtmlCompleted;

        public event EventHandler<DownloadProgeressEventArgs> DownloadHtmlProgress;

        private readonly HybridDictionary userStateToLifetime;

        private Guid TaskId { get; set; }

        public List<Uri> Pages { get; set; }

        public MutipleCallHtmlClient()
        {
            _httpClient = new HttpClient();
            Pages = new List<Uri>();
            userStateToLifetime = new HybridDictionary();
            _downloadHtmlCompltedCallback = StartDownload;
            _downloadHtmlProgressCallback = StartProgess;
        }
        private void DownloadHtmlAsync(AsyncOperation asyncOperation)
        {
            var result = new Dictionary<Uri, long>();
            if(asyncOperation.UserSuppliedState == null || TaskCanceled(asyncOperation.UserSuppliedState))
            {
                return;
            }


            foreach (var page in Pages.Select((x, i) => new { Value = x, Index = i }))
            {
                if (TaskCanceled(asyncOperation.UserSuppliedState))
                {
                    break;
                }

                var html = DowloadHtml(page.Value);
                result.Add(page.Value, html.Length);

                var progressComplete = new DownloadProgeressEventArgs(
                    null, TaskCanceled(asyncOperation.UserSuppliedState), asyncOperation.UserSuppliedState,
                    (int)((double)(page.Index + 1) / (double)Pages.Count * 100));
                asyncOperation.Post(_downloadHtmlProgressCallback, progressComplete);
            }

            var downloadComplete = new DownloadHtmlCompletedEvntArgs(null, TaskCanceled(asyncOperation.UserSuppliedState),
                asyncOperation.UserSuppliedState, result);
            asyncOperation.PostOperationCompleted(_downloadHtmlCompltedCallback, downloadComplete);
        }

        private bool TaskCanceled(object taskId)
        {
            lock (userStateToLifetime.SyncRoot)
            {
                return userStateToLifetime[taskId] == null;
            }
        }

        public void CancelAsync()
        {
            lock (userStateToLifetime.SyncRoot)
            {
                if (userStateToLifetime[TaskId] != null)
                {
                    userStateToLifetime.Remove(TaskId);
                }
            }
        }

        private void OnDownloadHtmlProgerss(DownloadProgeressEventArgs state)
        {
            DownloadHtmlProgress.Invoke(this, state);
        }

        private void StartProgess(object? state)
        {
            if (state == null)
            {
                return;
            }

            OnDownloadHtmlProgerss((DownloadProgeressEventArgs)state);
        }

        public void DownloadHtmlAsync()
        {
            TaskId = Guid.NewGuid();
            var asyncOperation = AsyncOperationManager.CreateOperation(TaskId);
            var callback = new DownloadHtmlCallback(DownloadHtmlAsync);

            lock (userStateToLifetime.SyncRoot)
            {
                if (userStateToLifetime.Contains(TaskId))
                {
                    throw new ArgumentException("Numer zadania musi być unikalny", nameof(TaskId));
                }

                userStateToLifetime.Add(TaskId, asyncOperation);
            }

            Task.Run(() => { callback(asyncOperation); });
        }

        private void StartDownload(object? state)
        {
            if(state == null)
            {
                return;
            }

            OnDownloadHtmlCompleted((DownloadHtmlCompletedEvntArgs)state);
        }

        private void OnDownloadHtmlCompleted(DownloadHtmlCompletedEvntArgs downloadHtmlCompletedEvntArgs)
        {
            DownloadHtmlCompleted.Invoke(this, downloadHtmlCompletedEvntArgs);
        }

        

        private string DowloadHtml(Uri address)
        {
            var webRequest = new HttpRequestMessage(HttpMethod.Get, address);
            var response = _httpClient.Send(webRequest);
            using var reader = new StreamReader(response.Content.ReadAsStream());

            return reader.ReadToEnd();
        }
    }
}

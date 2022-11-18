using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EAPWPFDemo.EventAsynchronusPattern
{
    public class DownloadHtmlCompletedEvntArgs : AsyncCompletedEventArgs
    {
        public DownloadHtmlCompletedEvntArgs(
            Exception? error,
            bool cancelled,
            object? userState,
            Dictionary<Uri, long> allData)
            : base(error, cancelled, userState)
        {
            AllData = allData;
        }

        public Dictionary<Uri, long> AllData { get; set; }
    }
}

using System;
using System.ComponentModel;

namespace EAPWPFDemo.EventAsynchronusPattern
{
    public class DownloadProgeressEventArgs : AsyncCompletedEventArgs
    {
        public DownloadProgeressEventArgs(Exception? error, bool cancelled, object? userState, int progess)
            : base(error, cancelled, userState)
        {
            Progeress = progess;
        }

        public int Progeress { get; set; }
    }
}

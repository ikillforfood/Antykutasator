using System;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Antykutasator.VideoCapture
{
    public interface IVideoCaptureService
    {
        void Start();
        void Stop();
        event EventHandler<NewFrameEventArgs> FrameCaptured;
    }
}

using System;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Antykutasator.VideoCapture
{
    public class VideoCaptureService : IVideoCaptureService
    {
        private const int CaptureInterval = 2; 
        private readonly VideoCaptureDeviceCollection _videoDevices;
        private VideoCaptureDevice _videoSource;
        private DateTime _lastCaptureTime;


        public VideoCaptureService(VideoCaptureDeviceCollection captureDeviceCollection)
        {
            _lastCaptureTime = DateTime.Now;
            // enumerate video devices
            _videoDevices = captureDeviceCollection;
        }

        public void Start(FilterInfo selectedVideoDevice = null)
        {
            // create video source
            _videoSource = selectedVideoDevice == null
                ? new VideoCaptureDevice(_videoDevices.GetDefaultDeviceName())
                : new VideoCaptureDevice(selectedVideoDevice.MonikerString);
            // set NewFrame event handler
            _videoSource.NewFrame += video_NewFrame;
            // start the video source
            _videoSource.Start();
        }

        public void Stop()
        {
            _videoSource?.SignalToStop();
        }

        public event EventHandler<NewFrameEventArgs> FrameCaptured;

        private void video_NewFrame(object sender, NewFrameEventArgs eventargs)
        {
            if (FrameCaptured == null || DateTime.Now - _lastCaptureTime < TimeSpan.FromSeconds(CaptureInterval))
            {
                return;
            }
            FrameCaptured(this, eventargs);
            _lastCaptureTime = DateTime.Now;
        }
    }
}

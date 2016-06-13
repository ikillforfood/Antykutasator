using System;
using AForge.Video;
using AForge.Video.DirectShow;
using Antykutasator.Helpers;

namespace Antykutasator.VideoCapture
{
    public class VideoCaptureService : IVideoCaptureService
    {
        private readonly VideoCaptureDeviceCollection _videoDevices;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private VideoCaptureDevice _videoSource;
        private DateTime _lastCaptureTime;


        public VideoCaptureService(VideoCaptureDeviceCollection captureDeviceCollection,
            ApplicationConfiguration applicationConfiguration)
        {
            _lastCaptureTime = DateTime.Now;
            // enumerate video devices
            _videoDevices = captureDeviceCollection;
            _applicationConfiguration = applicationConfiguration;
        }

        public void Start()
        {
            // create video source
            _videoSource = _applicationConfiguration.SelectedVideoCaptureDevice == null
                ? new VideoCaptureDevice(_videoDevices.GetDefaultDeviceName())
                : new VideoCaptureDevice(_applicationConfiguration.SelectedVideoCaptureDevice.MonikerString);
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
            if (FrameCaptured == null || DateTime.Now - _lastCaptureTime < TimeSpan.FromSeconds(_applicationConfiguration.FrameCaptureInterval))
            {
                return;
            }
            FrameCaptured(this, eventargs);
            _lastCaptureTime = DateTime.Now;
        }
    }
}

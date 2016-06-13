using System;
using AForge.Video.DirectShow;

namespace Antykutasator.Helpers
{
    public class ApplicationConfiguration
    {
        public FilterInfo SelectedVideoCaptureDevice { get; set; }

        public bool ScreenLocked { get; set; }

        public int FrameCaptureInterval { get; set; }

        public int FaceNotDetectedLimit { get; set; }

        public TimeSpan DeviceInactivityLimit { get; set; }

        public ApplicationConfiguration()
        {
            FrameCaptureInterval = 10;
            FaceNotDetectedLimit = 3;
            DeviceInactivityLimit = TimeSpan.FromSeconds(5);
        }
    }
}

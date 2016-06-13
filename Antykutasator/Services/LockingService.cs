using System;
using System.Diagnostics;
using Antykutasator.FaceDetection;
using Antykutasator.Helpers;
using Microsoft.Win32;
using Utils.Asynchronous;

namespace Antykutasator.Services
{
    public class LockingService : ILockingService
    {
        private readonly IMouseService _mouseService;
        private readonly IKeyboardService _keyboardService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private int _faceNotDetectedInRow = 0;

        public LockingService(IMediator mediator,
            IMouseService mouseService,
            IKeyboardService keyboardService,
            ApplicationConfiguration applicationConfiguration)
        {
            mediator.RegisterAsync<FaceDetectionResult>(this, FaceDetectionResultHandler);
            _mouseService = mouseService;
            _keyboardService = keyboardService;

            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            _applicationConfiguration = applicationConfiguration;
        }

        public void LockWorkstation()
        {
            NativeMethodsHelper.LockWorkStation();
        }

        private void FaceDetectionResultHandler(FaceDetectionResult args)
        {
            Debug.WriteLine(args.ToString());
            if (args == FaceDetectionResult.FaceNotFound)
            {
                _faceNotDetectedInRow++;
            }
            else
            {
                _faceNotDetectedInRow = 0;
            }

            if (_faceNotDetectedInRow < _applicationConfiguration.FaceNotDetectedLimit)
            {
                return;
            }
            if (DateTime.Now - _mouseService.LastDeviceActivityTime < _applicationConfiguration.DeviceInactivityLimit ||
                DateTime.Now - _keyboardService.LastDeviceActivityTime < _applicationConfiguration.DeviceInactivityLimit) return;
            LockWorkstation();
            _faceNotDetectedInRow = 0;
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                _applicationConfiguration.ScreenLocked = true;
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                _applicationConfiguration.ScreenLocked = false;
            }
        }
    }
}

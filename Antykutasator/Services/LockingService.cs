using System;
using System.Diagnostics;
using Antykutasator.FaceDetection;
using Antykutasator.Helpers;
using Utils.Asynchronous;

namespace Antykutasator.Services
{
    public class LockingService : ILockingService
    {
        private const int FaceNotDetectedLimit = 5;
        private readonly TimeSpan _deviceInactivityLimit;
        private readonly IMouseService _mouseService;
        private readonly IKeyboardService _keyboardService;
        private int _faceNotDetectedInRow = 0;

        public LockingService(IMediator mediator,
            IMouseService mouseService,
            IKeyboardService keyboardService)
        {
            mediator.RegisterAsync<FaceDetectionResult>(this, FaceDetectionResultHandler);
            _mouseService = mouseService;
            _keyboardService = keyboardService;
            _deviceInactivityLimit = new TimeSpan(0, 0, 0, 5);
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

            if (_faceNotDetectedInRow < FaceNotDetectedLimit)
            {
                return;
            }
            if (DateTime.Now - _mouseService.LastDeviceActivityTime < _deviceInactivityLimit ||
                DateTime.Now - _keyboardService.LastDeviceActivityTime < _deviceInactivityLimit) return;
            LockWorkstation();
            _faceNotDetectedInRow = 0;
        }
    }
}

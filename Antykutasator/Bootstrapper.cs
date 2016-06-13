using Antykutasator.FaceDetection;
using Antykutasator.Helpers;
using Antykutasator.Services;
using Antykutasator.VideoCapture;
using Catel.IoC;
using Utils.Asynchronous;

namespace Antykutasator
{
    public class Bootstrapper
    {
        public static void Initialize()
        {
            var serviceLocator = ServiceLocator.Default;

            serviceLocator.RegisterType<IMediator, Mediator>();
            serviceLocator.RegisterType<IFaceDetector, FaceDetector>();
            serviceLocator.RegisterType<ApplicationConfiguration>();
            serviceLocator.RegisterType<IDispatcherService, DispatcherService>();
            serviceLocator.RegisterType<IMouseService, MouseService>();
            serviceLocator.RegisterType<IKeyboardService, KeyboardService>();
            serviceLocator.RegisterTypeAndInstantiate<ILockingService, LockingService>();
            serviceLocator.RegisterType<VideoCaptureDeviceCollection>();
            serviceLocator.RegisterType<IVideoCaptureService, VideoCaptureService>();
            serviceLocator.RegisterType<ApplicationCommands>();
            serviceLocator.RegisterType<ApplicationStateMachine>();
            serviceLocator.RegisterType<ApplicationProcess>();
            serviceLocator.RegisterType<ApplicationProcessExecutor>();
        }
    }
}

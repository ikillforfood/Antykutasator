using Antykutasator.FaceDetection;
using Antykutasator.Helpers;
using Antykutasator.VideoCapture;
using Utils.Asynchronous;

namespace Antykutasator
{
    public class ApplicationCommands
    {
        private readonly IVideoCaptureService _videoCaptureService;
        private readonly IFaceDetector _faceDetector;
        private readonly IMediator _mediator;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public ApplicationCommands(IVideoCaptureService videoCaptureService,
            IFaceDetector faceDetector,
            IMediator mediator,
            ApplicationConfiguration applicationConfiguration)
        {
            _videoCaptureService = videoCaptureService;
            _faceDetector = faceDetector;
            _mediator = mediator;
            _applicationConfiguration = applicationConfiguration;
        }

        public void StartFaceDetection()
        {
            _videoCaptureService.FrameCaptured += _videoCaptureService_FrameCaptured;
            _videoCaptureService.Start(_applicationConfiguration.SelectedVideoCaptureDevice);
        }

        public void StopFaceDetection()
        {
            _videoCaptureService.Stop();
            _videoCaptureService.FrameCaptured -= _videoCaptureService_FrameCaptured;
        }

        public void ChangeVideoCaptureDevice()
        {
            _videoCaptureService.Stop();
            _videoCaptureService.Start(_applicationConfiguration.SelectedVideoCaptureDevice);
        }

        private void _videoCaptureService_FrameCaptured(object sender, AForge.Video.NewFrameEventArgs e)
        {
            var result = _faceDetector.Process(e.Frame);
            _mediator.SendMessageAsync(this, result);
        }
    }
}

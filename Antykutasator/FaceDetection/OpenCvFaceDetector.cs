using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Antykutasator.FaceDetection
{
    public class OpenCvFaceDetector : IFaceDetector
    {
        private CascadeClassifier _cascadeClassifier;

        public OpenCvFaceDetector()
        {
            _cascadeClassifier = new CascadeClassifier("Resources/haarcascades/haarcascade_frontalface_alt_tree.xml");
        }

        public FaceDetectionResult Process(Bitmap picture)
        {
            using (var grayframe = new Image<Gray, byte>(picture))
            {
                var faces = _cascadeClassifier.DetectMultiScale(grayframe, 1.1, 10, Size.Empty);
                if (faces.Length <= 0)
                {
                    return FaceDetectionResult.FaceNotFound;
                }
                return faces.Length > 1 ? FaceDetectionResult.MultipleFacesFound : FaceDetectionResult.OneFaceFound;
            }
        }
    }
}

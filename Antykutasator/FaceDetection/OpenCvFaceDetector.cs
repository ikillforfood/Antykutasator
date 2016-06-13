using System;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Antykutasator.FaceDetection
{
    public class OpenCvFaceDetector : IFaceDetector, IDisposable
    {
        private readonly CascadeClassifier _frontFaceCascadeClassifier;
        private readonly CascadeClassifier _profileCascadeClassifier;

        public OpenCvFaceDetector()
        {
            _frontFaceCascadeClassifier = new CascadeClassifier("Resources/haarcascades/haarcascade_frontalface_alt_tree.xml");
            _profileCascadeClassifier = new CascadeClassifier("Resources/haarcascades/haarcascade_profileface.xml");
        }

        public FaceDetectionResult Process(Bitmap picture)
        {
            using (var grayframe = new Image<Gray, byte>(picture).Resize(320, 240, Inter.Cubic))
            {
                var result = FaceDetectionResult.FaceNotFound;

                var frontfaces = _frontFaceCascadeClassifier.DetectMultiScale(grayframe, 1.1, 10, Size.Empty);
                result = frontfaces.Length > 0 ? FaceDetectionResult.OneFaceFound : FaceDetectionResult.FaceNotFound;
                if (result != FaceDetectionResult.FaceNotFound)
                {
                    Debug.WriteLine("front face found");
                    return result;
                }

                var leftProfile = _profileCascadeClassifier.DetectMultiScale(grayframe, 1.1, 10, Size.Empty);
                result = leftProfile.Length > 0 ? FaceDetectionResult.OneFaceFound : FaceDetectionResult.FaceNotFound;
                if (result != FaceDetectionResult.FaceNotFound)
                {
                    Debug.WriteLine("left profile found");
                    return result;
                }
                picture.RotateFlip(RotateFlipType.Rotate180FlipY);
                var rotatedFrame = new Image<Gray, byte>(picture).Resize(320, 240, Inter.Cubic);
                var rightProfile = _profileCascadeClassifier.DetectMultiScale(rotatedFrame, 1.1, 10, Size.Empty);
                rotatedFrame.Dispose();
                result = rightProfile.Length > 0 ? FaceDetectionResult.OneFaceFound : FaceDetectionResult.FaceNotFound;

                Debug.WriteLine(result == FaceDetectionResult.FaceNotFound ? "face not found" : "right profile found");
                return result;
            }
        }

        public void Dispose()
        {
            _frontFaceCascadeClassifier.Dispose();
            _profileCascadeClassifier.Dispose();
        }
    }
}

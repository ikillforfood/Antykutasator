using System;
using System.Drawing;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;

namespace Antykutasator.FaceDetection
{
    public class FaceDetector : IFaceDetector
    {
        HaarObjectDetector detector;

        public FaceDetector()
        {
            HaarCascade cascade = new FaceHaarCascade();
            detector = new HaarObjectDetector(cascade, 30);

            detector.SearchMode = ObjectDetectorSearchMode.Default;
            detector.ScalingMode = ObjectDetectorScalingMode.GreaterToSmaller;
            detector.ScalingFactor = 1.1f;
            detector.UseParallelProcessing = true;
            detector.Suppression = 2;
        }

        public FaceDetectionResult Process(Bitmap picture)
        {
            try
            {
                // Process frame to detect objects
                Rectangle[] objects = detector.ProcessFrame(picture);
                if (objects.Length == 0)
                {
                    return FaceDetectionResult.FaceNotFound;
                }
                return objects.Length > 1 ? FaceDetectionResult.MultipleFacesFound : FaceDetectionResult.OneFaceFound;
            }
            catch (Exception e)
            {
                return FaceDetectionResult.Error;
            }
        }
    }
}

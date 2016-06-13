using System.Drawing;

namespace Antykutasator.FaceDetection
{
    public interface IFaceDetector
    {
        FaceDetectionResult Process(Bitmap picture);
    }
}

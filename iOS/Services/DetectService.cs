using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Vision;
using CoreImage;
using CoreFoundation;
using CoreML.Services;

namespace CoreML.iOS.Services
{
    public class DetectService : IDetectService
    {
        private static VNCoreMLModel VModel { get; }

        static DetectService()
        {
            // Load the ML model
            var assetPath = NSBundle.MainBundle
                                    .GetUrlForResource(name: "detectBalls", fileExtension: "mlmodelc");
            var detectModel = MLModel.Create(url: assetPath, error: out _);
            VModel = VNCoreMLModel.FromMLModel(model: detectModel, error: out _);
        }

        public Task<string> DetectAsync(byte[] image)
        {
            var taskSource = new TaskCompletionSource<string>();

            void handleClassification(VNRequest request, NSError error)
            {
                var observations = request.GetResults<VNClassificationObservation>();

                if (observations == null)
                {
                    taskSource.SetException(exception: new Exception(message: "Unexpected result type from VNCoreMLRequest"));
                    return;
                }

                if (observations.Length == 0)
                {
                    taskSource.SetResult(result: null);
                    return;
                }

                var bestObservations = observations.First();
                taskSource.SetResult(result: bestObservations.Identifier);
            }

            using (var data = NSData.FromArray(buffer: image))
            {
                var ciImage = new CIImage(data: data);
                var handler = new VNImageRequestHandler(image: ciImage, imageOptions: new VNImageOptions());

                DispatchQueue.DefaultGlobalQueue.DispatchAsync(() => 
                {
                    handler.Perform(requests: new VNRequest[] {
                        new VNCoreMLRequest(model: VModel, completionHandler: handleClassification)
                    }, error: out _);
                });
            }
            return taskSource.Task;
        }
    }
}

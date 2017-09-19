using System;
using System.Threading.Tasks;
using CoreML.Services;
using System.Net;

[assembly: Xamarin.Forms.Dependency(typeof(CoreML.Droid.Services.DetectService))]
namespace CoreML.Droid.Services
{
    public class DetectService : IDetectService
    {
        public DetectService()
        {
        }

        // Use REST API. Not local ML model like iOS11
        public Task<string> DetectAsync(byte[] image)
        {
            var d = new CoreML.Services.DetectService();
            return d.DetectAsync(image);
        }
    }
}

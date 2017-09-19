using System;
using System.Threading.Tasks;
using CoreML.Services;

namespace CoreML.iOS.Services
{
    public class DetectService : IDetectService
    {
        public DetectService()
        {
        }

        public Task<string> DetectAsync(byte[] image)
        {
            throw new NotImplementedException();
        }
    }
}

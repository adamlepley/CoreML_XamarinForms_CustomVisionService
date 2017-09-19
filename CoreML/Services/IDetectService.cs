using System;
using System.Threading.Tasks;

namespace CoreML.Services
{
    public interface IDetectService
    {
        Task<string> DetectAsync(byte[] image);
    }
}

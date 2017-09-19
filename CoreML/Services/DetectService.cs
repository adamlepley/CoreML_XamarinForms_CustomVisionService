using System;
using System.Threading.Tasks;
using CoreML.Services;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CoreML.Models;
using System.Collections.Generic;

namespace CoreML.Services
{
    public class DetectService : IDetectService
    {
        public DetectService()
        {
        }

        // Use REST API. Not local ML model like iOS11
        public async Task<string> DetectAsync(byte[] image)
        {
            return (await DetectResultAsync(image)).Predictions[0].Tag;
        }

        public async Task<DetectResultModel.DetectResult> DetectResultAsync(byte[] image)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(name: "Prediction-Key", value: Consts.ApiEndPoints.PredictionKey);

                var content = new ByteArrayContent(content: image);
                content.Headers.ContentType = new MediaTypeHeaderValue(mediaType: "application/octet-stream");
                var result = await client.PostAsync(requestUri: Consts.ApiEndPoints.ApiEndpoint, content: content);
                result.EnsureSuccessStatusCode();
                var json = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DetectResultModel.DetectResult>(json);
            }
        }
    }
}

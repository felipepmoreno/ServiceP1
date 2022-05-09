using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceP1.Controllers
{
    [Route("api/p1/create-hashes")]
    [ApiController]
    public class GenerateTokensController : ControllerBase
    {
        public HttpClient client = new HttpClient();
        public List<string> tokensList = new List<string>();
        public Random random = new Random();
        public P1BodyModel model = new P1BodyModel();
        public int count = 0;
        public Stopwatch stopwatch;

        [HttpGet]
        public async Task<List<string>> P1CreateHashes()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.ElapsedMilliseconds < 5000)
            {
                model.n = random.Next(5000, 15000);
                model.code = random.Next(10000000) + 10000000;

                var jsonModel = JsonConvert.SerializeObject(model);
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5001/api/p2/validate");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(jsonModel, Encoding.UTF8);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.SendAsync(request).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                tokensList.Add(content);
            }
            stopwatch.Stop();
            Console.WriteLine("Foram geradas " + tokensList.Count() + " hashes");
            return tokensList;
        }
    }
}

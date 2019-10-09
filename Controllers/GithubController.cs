using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using webhookstest.Models;
using webhookstest.Models.GitHubWebHookApi.Models;

namespace webhookstest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubController : ControllerBase
    {
        // POST: api/Github
        [HttpPost]
        public async Task PostAsync(GitHubPayLoad githubPayload)
        {
            //var json = githubPayload;


            //var pullCommentUrl = JsonConvert.SerializeObject(githubPayload);
            var pullCommentUrl = githubPayload.Pull_request.Review_comments_url;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.github.com");
            var token = "96c8818c7a4b8fc7e60c93fc4f6349d0c563386a";
            client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);

            var response = await client.GetAsync(new Uri(pullCommentUrl).LocalPath);
            string commentForFile = "";
            if (response.IsSuccessStatusCode)
            {
               var responseString = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<IEnumerable<CommentPayload>>(responseString).ToList<CommentPayload>();
                comments.ForEach(comment => commentForFile += comment.User.Login+ "\n" +comment.Body + "\n\n");
                commentForFile += $"Total Comments :{comments.Count}";
                   
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                commentForFile = $"Error In Fetching Data fro api call {response.ReasonPhrase}";



            }
            System.IO.File.WriteAllText(@"C:\Users\tsharma\source\repos\webhookstest\webhookstest\response.txt", commentForFile);



        }
    }
}

using System.Collections.Generic;
using System.Net.Http;
using System.Web.Configuration;
using github.jobs.app.Models;
using Newtonsoft.Json;

namespace github.jobs.app.Services
{
    public class JobService
    {
        public PagedViewModel<JobModel> Search(string description, string location, bool fullTime, int page = 0, bool calculateTotal = false)
        {
            string body = GetResponse(FormatUrl(description, location, fullTime, page));
            PagedViewModel<JobModel> lookup = new PagedViewModel<JobModel> { Page = page };
            List<JobModel> jobs = JsonConvert.DeserializeObject<List<JobModel>>(body);
            lookup.Items = jobs;
            if (calculateTotal)
            {
                lookup.Total = jobs.Count;
                lookup.Total += CalculateTotal(description, location, fullTime);
            }

            return lookup;
        }

        private int CalculateTotal(string description, string location, bool fullTime)
        {
            int counter = 2;
            int amount = 1;
            int total = 0;
            while (amount != 0)
            {
                string body = GetResponse(FormatUrl(description, location, fullTime, counter));
                var jobs = JsonConvert.DeserializeObject<List<JobModel>>(body);
                amount = jobs.Count;
                total += amount;
                counter++;
            }

            return total;
        }

        private string GetResponse(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return responseBody;
        }

        private string FormatUrl(string description, string location, bool fullTime, int page = 0)
        {
            return $"{WebConfigurationManager.AppSettings["GithubJobsApi"]}description={description}&location={location}&full_time={fullTime}&page={page}";
        }
    }
}
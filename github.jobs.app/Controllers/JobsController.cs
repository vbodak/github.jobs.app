using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using github.jobs.app.Services;

namespace github.jobs.app.Controllers
{
    public class JobsController : ApiController
    {
        private readonly JobService _jobService;

        public JobsController()
        {
            _jobService = new JobService();
        }

        // GET api/jobs
        public HttpResponseMessage Get(string description = "", string location = "", bool fullTime = false, int page = 0, bool calculateTotal = false)
        {
            var model = _jobService.Search(description, location, fullTime, page, calculateTotal);
            return Request.CreateResponse(HttpStatusCode.OK, model, new JsonMediaTypeFormatter());
        }
    }
}

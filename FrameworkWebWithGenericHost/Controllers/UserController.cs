using System.Web.Http;
using System.Threading.Tasks;
using MassTransit;
using Entities.Commands;
using Entities.Events;

namespace FrameworkWebWithGenericHost.Controllers
{
    public class UserController : ApiController
    {
        private readonly IRequestClient<CreateUser> client;

        public UserController(IRequestClient<CreateUser> client)
        {
            this.client = client;
        }

        // GET: api/User
        public async Task<UserCreated> Get()
        {
            var response = await client.GetResponse<UserCreated>(new CreateUser(NewId.NextGuid(), "admin"));
            return response.Message;
        }

        // GET: api/User/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/User
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}

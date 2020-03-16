using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicrShopping.Identity.Controlls
{
    [Route("[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        // GET: /User
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string name = User.Identity.Name;
            return new string[] { "value1", "value2" };
        }

        // GET: User/5
        [HttpGet("{id}", Name = "Get")]
        [Authorize]
        public string Get(int id)
        {
            string name = User.Identity.Name;

            return "value";
        }

        // POST: /User
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: /User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: /ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

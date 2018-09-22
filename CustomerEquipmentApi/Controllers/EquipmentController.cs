using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Core.Handler;

namespace CustomerEquipmentApi.Controllers
{
    //  [Produces("application/json")]
    [Route("api/[controller]")]
    public class EquipmentController : ControllerBase
    {
        IEquipmentsHandler _equipmentHandler;

        public EquipmentController(IEquipmentsHandler equipmentHandler)
        {
            _equipmentHandler = equipmentHandler;
           
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            System.Diagnostics.Trace.WriteLine("Test, EquipmentController");
            System.Diagnostics.Trace.WriteLine("New test, EquipmentController...");
            return Ok(_equipmentHandler.getListOfEquipments());
        }

        /*
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}

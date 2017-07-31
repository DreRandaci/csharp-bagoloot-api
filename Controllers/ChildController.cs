using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BagAPI.Data;
using BagAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// In an ASP.NET application, the namespace will often, but does not need to,
// match the name of the directory in which the class is contained.
namespace BagAPI.Controllers
{
    [Route("api/[controller]")]
    public class ChildController : Controller
    {
        private BagAPIContext _context;
        private ILogger _log;

        /*
            Dependency injection works in ASP.NET exactly like it did with Angular.

            In this controller, I'm injecting...
                1. Entity Framework database context 
                2. The logging service, so I can write log messages without 
                   resorting to Console.WriteLine.
         */
        public ChildController(BagAPIContext ctx, ILogger<ChildController> logger)
        {
            _context = ctx;
            _log = logger;
        }

        /*
            Author: Steve Brownlee
            URL: GET api/child
            Description:
            This method handles GET requests for the child resource. There
            are two URL parameters that can be used to filter the list of 
            children returned.

                /api/child?delivered=0
                /api/child?name=Svetlana
                /api/child?delivered=0&name=Nigel
         */
        [HttpGet]
        public async Task<IActionResult> Get(int? delivered, string name)
        {
            IEnumerable<Child> children;

            // Store URL parameters in a tuple, just because I can
            (string Name, int? Delivered) filter = (name, delivered);

            // If delivered was not a parameter at all
            if (filter.Delivered == null)
            {
                children = await _context.Child.Include("Toys").ToListAsync();

            // Delivered was a parameter. Use it to filter query.
            } else {
                children = await _context.Child
                        .Include("Toys")
                        .Where(c => c.Delivered == filter.Delivered)
                        .ToListAsync();
            }

            // If name was specified, refine query further
            if (filter.Name != null)
            {
                children = children.Where(c => c.Name.Contains(filter.Name));
            }

            // No children were found, send response with 404 status code
            if (children == null)
            {
                return NotFound();
            }

            // Send response with 200 status code and newly created child in body
            return Ok(children);
        }

        /*
            Author: Steve Brownlee
            URL: GET api/child/1
            Description: This method handles GET requests for a single child resource.
         */
        [HttpGet("{id}", Name = "GetChild")]
        public IActionResult Get([FromRoute] int id)
        {
            /*
                This condition validates the values in model binding.
                In this case, it validates that the id value is an integer.
                If the following URL is requested, model validation will
                fail - because the string of 'chicken' is not an integer -
                and the client will receive a message to that effect.

                    curl -X GET http://localhost:5000/api/child/chicken
             */
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Child child = _context.Child.Include("Toys").Single(m => m.ChildId == id);

                if (child == null)
                {
                    return NotFound();
                }
                
                return Ok(child);
            }
            catch (System.InvalidOperationException ex)
            {
                
                _log.LogDebug(ex.Message);
                return NotFound();
            }
        }

        /*
            Author: Steve Brownlee
            URL: POST api/child/1
            Description: This method handles POST requests to create a new child.
         */
        [HttpPost]
        public IActionResult Post([FromBody] Child child)
        {
            /*
                Model validation works differently here, since there
                is a complex type being detected with ([FromBody] Child child).
                This method will extract the key/value pairs from the JSON
                object that is posted, and create a new instance of the Child
                model class, with the corresponding properties set.

                If any of the validations fail, such as length of string values,
                if a value is required, etc., then the API will respond that
                it is a bad request.
             */
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Add the child to the database context
            _context.Child.Add(child);
            
            try
            {
                // Commit the newly created child to the database
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ChildExists(child.ChildId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            /*
                The CreatedAtRoute method will return the newly created child in the
                body of the response, and the Location meta-data header will contain
                the URL for the new child resource

                To see this run this from command line:
                    curl --header "Content-Type: application/json" -s -X POST -D - --data '{"name":"Billy Baxter"}' http://localhost:5000/api/child

                You will see this response:
                    HTTP/1.1 201 Created
                    Date: Mon, 31 Jul 2017 01:04:58 GMT
                    Transfer-Encoding: chunked
                    Content-Type: application/json; charset=utf-8
                    Location: http://localhost:5000/api/Child/4
                    Server: Kestrel

                    {"childId":4,"name":"Billy Baxter","delivered":0,"toys":null}
             */
            return CreatedAtRoute("GetChild", new { id = child.ChildId }, child);
        }

        /*
            Author: Steve Brownlee
            URL: POST api/child/create
            Description:
            Custom route that is outside the conventions used by ASP.NET to determine
            the routing for each resource. Also notice that the model binding is to
            the ChildToy class. It is not a table in the database since I don't add
            a DBSet of it in the BagAPIContext.cs file.

            Example POST body:
                {
                    "toyname": "Hot wheels",
                    "childname": "Samantha Young"
                }
         */
        [HttpPost("/api/Child/Create")]
        public IActionResult Post([FromBody] ChildToy childToy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create the child and add to the context
            Child child = new Child(){ Name=childToy.ChildName };
            _context.Child.Add(child);

            // Create the toy and add to the context
            Toy toy = new Toy(){
                Name=childToy.ToyName, 
                ChildId=child.ChildId
            };
            _context.Toy.Add(toy);
            
            try
            {
                // Commit both new records to the database at once
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ChildExists(child.ChildId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetChild", new { id = child.ChildId }, child);
        }

        /*
            Author: Steve Brownlee
            URL: PUT api/child/5
            Description:
            Handles the updating of a child record. Remember that a PUT requires
            you to send the entire object in the request, not just the field that
            you want to change.

            Example PUT body:
                {
                    "childid": 5,
                    "name": "Samantha Young",
                    "delivered": 0
                }
         */
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Child child)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != child.ChildId)
            {
                return BadRequest();
            }

            _context.Entry(child).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChildExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        /*
            Author: Steve Brownlee
            URL: DELETE api/child/5
            Description:
            Handles the deletion of a child record.
         */
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Child child = _context.Child.Single(m => m.ChildId == id);
            if (child == null)
            {
                return NotFound();
            }

            _context.Child.Remove(child);
            _context.SaveChanges();

            return Ok(child);
        }

        private bool ChildExists(int kidId)
        {
          return _context.Child.Count(e => e.ChildId == kidId) > 0;
        }
    }
}

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
            GET api/child

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

        // GET api/child/5
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

        // POST api/child
        [HttpPost]
        public IActionResult Post([FromBody] Child child)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Child.Add(child);
            
            try
            {
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

        [HttpPost("/api/Child/Create")]
        public IActionResult Post([FromBody] ChildToy childToy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Child child = new Child(){Name=childToy.ChildName};
            _context.Child.Add(child);

            Toy toy = new Toy(){Name=childToy.ToyName, ChildId=child.ChildId};
            _context.Toy.Add(toy);
            
            try
            {
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

        private bool ChildExists(int kidId)
        {
          return _context.Child.Count(e => e.ChildId == kidId) > 0;
        }

        // PUT api/child/5
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

        // DELETE api/child/5
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
    }
}

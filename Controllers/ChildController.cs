using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BagAPI.Data;
using BagAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BagAPI.Controllers
{
    [Route("api/[controller]")]
    public class ChildController : Controller
    {
        private BagAPIContext _context;
        public ChildController(BagAPIContext ctx)
        {
            _context = ctx;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get(bool delivered)
        {
            IQueryable<object> children;

            if (delivered)
            {
                int del = (delivered) ? 1 : 0;
                children = _context.Child.Include("Toys").Where(c => c.Delivered == del);
            } else {
                children = _context.Child.Include("Toys");
            }


            if (children == null)
            {
                return NotFound();
            }

            return Ok(children);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetChild")]
        public IActionResult Get([FromRoute] int id)
        {
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

        // PUT api/values/5
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

        // DELETE api/values/5
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

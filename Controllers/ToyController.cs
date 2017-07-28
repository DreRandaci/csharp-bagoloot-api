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
    public class ToyController : Controller
    {
        private BagAPIContext _context;
        public ToyController(BagAPIContext ctx)
        {
            _context = ctx;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<object> toys = _context.Toy;

            if (toys == null)
            {
                return NotFound();
            }

            return Ok(toys);

        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetToy")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Toy toy = _context.Toy.Single(m => m.ToyId == id);

                if (toy == null)
                {
                    return NotFound();
                }
                
                return Ok(toy);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Toy toy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Toy.Add(toy);
            
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ToyExists(toy.ToyId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetToy", new { id = toy.ToyId }, toy);
        }

        private bool ToyExists(int toyId)
        {
        return _context.Toy.Count(e => e.ToyId == toyId) > 0;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Toy toy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != toy.ToyId)
            {
                return BadRequest();
            }

            _context.Entry(toy).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToyExists(id))
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

            Toy toy = _context.Toy.Single(m => m.ToyId == id);
            if (toy == null)
            {
                return NotFound();
            }

            _context.Toy.Remove(toy);
            _context.SaveChanges();

            return Ok(toy);
        }
    }
}

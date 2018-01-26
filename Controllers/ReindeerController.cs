using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BagoLootAPI.Data;
using BagoLootAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BagoLootAPI.Controllers
{
    [Route("api/[controller]")]
    public class ReindeerController : Controller
    {
        private ApplicationDbContext _context;
        public ReindeerController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        // GET api/reindeer
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<object> reindeer = _context.Reindeer.Include("Fans.Child");

            if (reindeer == null)
            {
                return NotFound();
            }

            return Ok(reindeer);
        }

        // GET api/reindeer/5
        [HttpGet("{id}", Name = "GetReindeer")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Reindeer Reindeer = _context.Reindeer.Include("Fans.Child").Single(m => m.ReindeerId == id);

                if (Reindeer == null)
                {
                    return NotFound();
                }

                return Ok(Reindeer);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }
        }

        // POST api/Reindeer
        [HttpPost]
        public IActionResult Post([FromBody] Reindeer Reindeer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Reindeer.Add(Reindeer);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ReindeerExists(Reindeer.ReindeerId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetReindeer", new { id = Reindeer.ReindeerId }, Reindeer);
        }

        private bool ReindeerExists(int deerId)
        {
          return _context.Reindeer.Count(e => e.ReindeerId == deerId) > 0;
        }

        // PUT api/reindeer/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Reindeer Reindeer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Reindeer.ReindeerId)
            {
                return BadRequest();
            }

            _context.Entry(Reindeer).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReindeerExists(id))
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

        // DELETE api/reindeer/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Reindeer Reindeer = _context.Reindeer.Single(m => m.ReindeerId == id);
            if (Reindeer == null)
            {
                return NotFound();
            }

            _context.Reindeer.Remove(Reindeer);
            _context.SaveChanges();

            return Ok(Reindeer);
        }
    }
}

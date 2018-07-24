using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Produces("application/json")]
    [Route("api/Dish")]
    public class DishController : ControllerBase
    {
        private AppDbContext _db;

        public DishController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet(Name = "Dish.GetAll")]
        public async Task<List<Dish>> GetAll()
        {//Include - responsavel por trazer o relacionamento

            return await  _db.Dishes.OrderBy(r => r.Name).ToListAsync();
           

          
        }


        [HttpGet("{id}", Name = "Dish.Get")]
        public async Task<IActionResult> GetId(int id)
        {
            try
            {
                var dish = await _db.Dishes.FindAsync(id);

                if (dish == null)
                {
                    return NotFound();
                }

                return Ok(dish);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost(Name = "Dish.Create")]
        public async Task<IActionResult> Create([FromBody] Dish newDish)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _db.Dishes.AddAsync(newDish);
                await _db.SaveChangesAsync();
                return CreatedAtRoute("Dish.Get", new { id = newDish.Id }, newDish);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{id}", Name = "Dish.Update")]
        public async Task<IActionResult> Update(int id, [FromBody] Dish dishNew)
        {
            try
            {
                if (!ModelState.IsValid || dishNew.Id != id)
                {
                    return BadRequest(ModelState);
                }

                var dishUpdated = await _db.Dishes.FindAsync(id);
                if (dishUpdated == null)
                {
                    return NotFound();
                }
                dishUpdated.Name = dishNew.Name;
                dishUpdated.Price = dishNew.Price;
                dishUpdated.RestaurantId = dishNew.RestaurantId;
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}", Name = "Dish.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var dish = await _db.Dishes.FindAsync(id);
                if (dish == null)
                {
                    return NotFound();
                }

                _db.Dishes.Remove(dish);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500);

            }

        }

    }
}

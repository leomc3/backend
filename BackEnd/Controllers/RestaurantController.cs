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
    [Route("api/Restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly AppDbContext _db;

        public RestaurantController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet(Name = "Restaurant.GetAll")]
        public async Task<List<Restaurant>> GetAll()
        {
            //Include - responsavel por trazer o relacionamento
            return await _db.Restaurants.
                Include(p => p.Dishes).
                OrderBy(r => r.Name).
                ToListAsync();
        }

        [HttpGet("{id}", Name = "Restaurant.GetId")]
        public async Task<IActionResult> GetId(int id)
        {
            try
            {
                Restaurant restaurant = await _db.Restaurants.FindAsync(id);
                
                if (restaurant == null)
                {
                    return NotFound();
                }
                //carrega relacionamento
                await _db.Entry(restaurant).Collection(r => r.Dishes).LoadAsync();
                return Ok(restaurant);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        public async Task<IActionResult> Create([FromBody] Restaurant newRestaurant)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _db.Restaurants.AddAsync(newRestaurant);
                await _db.SaveChangesAsync();
                return Ok(newRestaurant);
            }
            catch (Exception )
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{id}", Name = "Restaurant.Updt")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update(int id, [FromBody] Restaurant restaurantNew)
        {
            try
            {
                if (!ModelState.IsValid || restaurantNew.Id != id)
                {
                    return BadRequest(ModelState);
                }
                

                var restaurantUpdated = _db.Restaurants.Find(id);

                if (restaurantUpdated == null)
                {
                    return NotFound();
                }
                restaurantUpdated.Name = restaurantNew.Name;
                await _db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception )
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}", Name = "Restaurant.Del")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var restaurant = await _db.Restaurants.FindAsync(id);
                if (restaurant == null)
                {
                    return NotFound();
                }

                _db.Restaurants.Remove(restaurant);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception )
            {
                return StatusCode(500);
            }
        }
    }
}
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
        [HttpGet]
        public async Task<List<Restaurant>> GetAll()
        {//Include - responsavel por trazer o relacionamento
            return await _db.Restaurants.Include(p => p.Dishes).OrderBy(r => r.Name).ToListAsync();
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
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

    }
}
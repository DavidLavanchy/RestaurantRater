using RestaurantRaterAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestaurantRaterAPI.Controllers
{
    public class RestaurantController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();

        //POST (create)
        //api/Restaurant
        [HttpPost]
        public async Task<IHttpActionResult> PostRestaurant([FromBody] Restaurant model)
        {
            if (model is null)
            {
                return BadRequest("Request body cannot be empty.");
            }

            if (ModelState.IsValid)
            {
                //Store model in database
                _context.Restaurants.Add(model);
                int changeCount = await _context.SaveChangesAsync();
                return Ok("Your Restaurant was created!");
            }

            //if not valid, then reject it
            return BadRequest(ModelState);
        }

        // Get ALL
        // api/Restaurant
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Restaurant> restaurants = await _context.Restaurants.ToListAsync();

            return Ok(restaurants);
        }


        // Get by ID
        //api.Restaurant/{id}
        [HttpGet]
        public async Task<IHttpActionResult> GetByID(int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant != null)
            {
                return Ok(restaurant);
            }
            return NotFound();
        }

        // Put (update)
        // api/Restaurant/{id}
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRestaurantmethod([FromUri] int id, [FromBody] Restaurant updatedRestaraunt)
        {
            // Check if they match
            if(id != updatedRestaraunt?.Id)
            {
                return BadRequest("Id's do not match");
            }

            //check model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //find restaraunt in database
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);
            //if restaurant doesn't exist
            if(restaurant is null)
            {
                return NotFound();
            }
            //update properties
            restaurant.Name = updatedRestaraunt.Name;
            restaurant.Address = updatedRestaraunt.Address;
            restaurant.Rating = updatedRestaraunt.Rating;

            //save the change
            await _context.SaveChangesAsync();

            return Ok("Restaraunt was updated.");
        }

        //Delete (delete)
        // api/Restaurant/{id}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRestaurant([FromUri]int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if(restaurant is null)
            {
                return NotFound();
            }

            _context.Restaurants.Remove(restaurant);

            if(await _context.SaveChangesAsync() == 1)
            {
                return Ok("Restaurant was deleted");
            }
            return InternalServerError();
        }
    }
}

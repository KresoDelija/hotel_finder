using Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace HotelFinderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class HotelAdminController : ControllerBase
    {
        private readonly ILogger<HotelAdminController> _logger;
        private readonly IHotelService _hotelService;

        public HotelAdminController(ILogger<HotelAdminController> logger
            , IHotelService hotelService
            )
        {
            _logger = logger;
            _hotelService = hotelService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all hotels", Description = "Listing all hotels in the system", Tags = new[] { "Hotel administration" })]
        public async Task<ActionResult> Get()
        {
            var allHotels = await new Domain.Model.Hotel().GetAll(_hotelService);
            return Ok(allHotels);
        }

        [HttpGet("{hotelId}")]
        [SwaggerOperation(Summary = "Get specific hotel", Description = "Get one hotel by it's ID", Tags = new[] { "Hotel administration" })]
        public async Task<ActionResult> Get(int hotelId)
        {
            var loadMe = new Domain.Model.Hotel() { Id = hotelId };
            var result = await loadMe.LoadData(_hotelService);

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add new hotel", Description = "Creates new hotel given name, price and location. Location is described with latitude and longitude.", Tags = new[] { "Hotel administration" })]
        public async Task<ActionResult> Post([FromBody] Domain.Model.HotelDetails hotel)
        {
            var newHotel = new Domain.Model.Hotel()
            {
                Name = hotel.Name,
                Price = hotel.Price,
                Location = hotel.Location
            };
            var result = await newHotel.Create(_hotelService);

            return Ok(result);
        }

        [HttpPut()]
        [SwaggerOperation(Summary = "Update hotel", Description = "Updates hotel details (name, price and location). Location is described with latitude and longitude", Tags = new[] { "Hotel administration" })]
        public async Task<ActionResult> Put([FromBody] Domain.Model.Hotel hotel)
        {
            var result = await hotel.Update(_hotelService);

            return Ok(result);
        }

        [HttpDelete("{hotelId}")]
        [SwaggerOperation(Summary = "Delete hotel", Description = "Deletes hotel by it's ID", Tags = new[] { "Hotel administration" })]
        public async Task<ActionResult> Delete(int hotelId)
        {
            var deleteMe = new Domain.Model.Hotel() { Id = hotelId};
            var result = await deleteMe.Delete(_hotelService);

            return Ok(result);
        }
    }
}

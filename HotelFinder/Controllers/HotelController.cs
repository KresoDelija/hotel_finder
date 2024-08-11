using Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;
        private readonly IHotelService _hotelService;

        public HotelController(ILogger<HotelController> logger
            , IHotelService hotelService
            )
        {
            _logger = logger;
            _hotelService = hotelService;
        }

        [HttpGet(Name = "FindHotels")]
        [SwaggerOperation(Summary = "Get hotels near location", Description = "Listing all hotels ordered by location and price. Returned set exposes Name, Price and Distance from given alocation in meters.", Tags = new[] { "Hotel c" })]
        public async Task<ActionResult> FindHotels(double longitude, double latitude, int? pageSize, int? pageNumber)
        {
            var allHotels = await new Domain.Model.HotelSearchResult().Search(_hotelService,
                new Domain.Model.LocationBase() 
                { 
                    Longitude = longitude, 
                    Latitude = latitude
                },
                pageSize,
                pageNumber
            );

            return Ok(allHotels);
        }
    }
}

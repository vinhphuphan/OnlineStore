// BuggyController is used for testing and demonstrating different types of HTTP error responses.
// Includes endpoints that simulate Unauthorized, BadRequest, NotFound, InternalServerError, and ValidationError.

using API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized() => Unauthorized();

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest() => BadRequest("This is a bad request!");

        [HttpGet("notfound")]
        public IActionResult GetNotFound() => NotFound();

        [HttpGet("internalerror")]
        public IActionResult GetInternalError() => throw new Exception("This is a test for internal error!");

        [HttpPost("validationerror")]
        public IActionResult GetValidationError(ProductDto product) => Ok();
    }
}

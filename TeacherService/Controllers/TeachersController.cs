using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TeacherService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        public TeachersController() { }

        [HttpPost]
        public async Task<IActionResult> AddTeacher()
        {
            try
            {

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Ok("");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MetoFirstExample_v4_WepAPI.Helpers;
using MetoFirstExample_v4_WepAPI.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MetoFirstExample_v4_WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly DatabaseHelper _databaseHelper;

        public ImageController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ImageModel image)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ImagePath", image.ImagePath),
                new SqlParameter("@CreatedAt", image.CreatedAt)
            };

            await _databaseHelper.ExecuteStoredProcedureAsync("sp_InsertImage", parameters);
            return Ok("Resim Kaydedildi. \n");
        }
    }
}

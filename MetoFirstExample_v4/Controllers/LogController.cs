using Microsoft.AspNetCore.Mvc;
using MetoFirstExample_v4_WepAPI.Helpers;
using MetoFirstExample_v4_WepAPI.Models;
using System.Data.SqlClient;

namespace MetoFirstExample_v4_WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly DatabaseHelper _databaseHelper;

        public LogController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LogModel log)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@LogType", log.LogType),
                new SqlParameter("@Message", log.Message),
                new SqlParameter("@CreatedAt", log.CreatedAt),
                new SqlParameter("@Source", log.Source)
            };
            await _databaseHelper.ExecuteStoredProcedureAsync("sp_InsertLog", parameters);
            return Ok("Log Kayıtlarına eklendi.\n");
        }
    }
}




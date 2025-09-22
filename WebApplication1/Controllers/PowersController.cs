using DemoApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowersController : ControllerBase
    {

        private readonly SqlHelper _sqlHelper;

        public PowersController(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Power>>> GetAll()
        {
            var dt = await _sqlHelper.ExecuteStoredProcedureAsync("sp_GetPowersByHero"
                ///new SqlParameter("@heroId", heroId)
                );
            var powers = dt.AsEnumerable().Select(row => new Power
            {
                Id = row.Field<int>("Id"),
                HeroId=row.Field<int>("HeroId"),
                Name = row.Field<string>("Name"),
                Description = row.Field<string>("Description")
            }).ToList();

            return Ok(powers);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Power power)
        {
            await _sqlHelper.ExecuteNonQueryAsync("sp_CreatePower",
                new SqlParameter("@HeroId", power.HeroId),
                new SqlParameter("@Name", power.Name),
                new SqlParameter("@Description", power.Description ?? (object)DBNull.Value));

            return Ok(new { message = "Power created successfully" });
        }

        [HttpPut]
        public async Task<IActionResult> Update(Power power)
        {
            await _sqlHelper.ExecuteNonQueryAsync("sp_UpdatePower",
                new SqlParameter("@id", power.Id),
                new SqlParameter("@HeroId", power.HeroId),
                new SqlParameter("@Name", power.Name),
                new SqlParameter("@Description", power.Description ?? (object)DBNull.Value));

            return Ok(new { message = "Power update successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _sqlHelper.ExecuteNonQueryAsync("sp_DeletePower",
                new SqlParameter("@Id", id));

            return Ok(new { message = "Power deleted successfully" });
        }



    }
}

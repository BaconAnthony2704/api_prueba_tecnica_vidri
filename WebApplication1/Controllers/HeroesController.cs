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
    public class HeroesController : ControllerBase
    {

        private readonly SqlHelper _sqlHelper;

        public HeroesController(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }


        // GET: api/<HeroesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hero>>> GetAll()
        {
            var dt = await _sqlHelper.ExecuteStoredProcedureAsync("sp_GetAllHeroes");
            var heroes = dt.AsEnumerable().Select(row => new Hero
            {
                Id = row.Field<int>("Id"),
                Name = row.Field<string>("Name"),
                Alias=row.Field<string>("Alias")
            }).ToList();

            return Ok(heroes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hero>> GetOne(int id)
        {
            var dt = await _sqlHelper.ExecuteStoredProcedureAsync(
                "sp_GetAllHeroes",
                new SqlParameter("@Id", id)
            );

            if (dt.Rows.Count == 0)
                return NotFound();

            var row = dt.Rows[0];
            var hero = new Hero
            {
                Id = (int)row["Id"],
                Name = row["Name"].ToString()!,
                Alias = row["Alias"].ToString()!
            };

            return Ok(hero);
        }


    }
}

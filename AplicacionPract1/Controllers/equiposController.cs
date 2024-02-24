using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AplicacionPract1.Models;
using System.Linq;

namespace AplicacionPract1.Properties
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {

        private readonly equiposContext _equiposContexto;

        public equiposController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {

            List<equipos> listadoEquipo = (from e in _equiposContexto.equipos select e).ToList();

            if (listadoEquipo.Count() == 0)
            {

                return NotFound();

            }
            return Ok(listadoEquipo);

}
    }
}

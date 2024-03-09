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

        //EndPoint que retorna el listado de todos los equipos existentes
        [HttpGet]
        [Route("GetInnerJoin")]

        public IActionResult Inner()
        {
            var listadoEquipo = (from e in _equiposContexto.equipos
                                 join t in _equiposContexto.tipo_equipo
                                        on e.tipo_equipo_id equals t.id_tipo_equipo
                                 join m in _equiposContexto.marcas
                                        on e.marca_id equals m.id_marcas
                                 join es in _equiposContexto.estados_equipo
                                        on e.estado_equipo_id equals es.id_estados_equipo
                                 select new
                                 {
                                     e.id_equipos,
                                     e.nombre,
                                     e.descripcion,
                                     e.tipo_equipo_id,
                                     tipo_equipo = t.descripcion,
                                     e.marca_id,
                                     marca = m.nombre_marcas,
                                     e.estado_equipo_id,
                                     estado_equipo = es.descripcion,
                                     detalle = $"Tipo : {t.descripcion}, Marca {m.nombre_marcas}, Estado Equipo {es.descripcion} ",
                                     e.estado
                                 }).Take(2).ToList();
            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipo);
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

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            equipos? equipo = _equiposContexto.equipos.FirstOrDefault(e => e.id_equipos == id);

            if (equipo == null)
            {
                return NotFound();
            }

            return Ok(equipo);
        }

        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult FindbyDescription(string filtro)
        {
            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }

            return Ok(equipo);
        }

        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarEquipo([FromBody] equipos equipo)
        {
            try
            {

                _equiposContexto.equipos.Add(equipo);
                _equiposContexto.SaveChanges();
                return Ok(equipo);

            }
            catch (Exception e) {

                return BadRequest(e.Message);

            }
        }

        [HttpPut]
        [Route("Actualizar/{id}")]

        public IActionResult ActualizarEquipo(int id, [FromBody] equipos equipoModificar)
        { 
        // Para alterar un registro, se obtiene el registro actual de la base de datos al cual alteraremos una propiedad
       
            equipos? equipoActual = _equiposContexto.equipos.FirstOrDefault(e => e.id_equipos == id);

            // verificamos que exista el registro segun su id
            if (equipoActual == null)
            {
                return NotFound();
            }

            // si se encuentra el registro se alteraran los campos modificables
            equipoActual.nombre = equipoModificar.nombre;
            equipoActual.descripcion = equipoModificar.descripcion;
            equipoActual.marca_id = equipoModificar.marca_id;
            equipoActual.tipo_equipo_id = equipoModificar.tipo_equipo_id;
            equipoActual.anio_compra = equipoModificar.anio_compra;
            equipoActual.costo = equipoModificar.costo;

            // se marca el registro como modificado en el contexto y se envia la modificacion a la bd
            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok();

        }

        [HttpDelete]
        [Route("Eliminar/{id}")]

        public IActionResult EliminarEquipo(int id)
        {
            //para actualizar un registro se obtiene el registro original de la base de datos al cual eliminaremos
            
            equipos? equipo = _equiposContexto.equipos.FirstOrDefault(e => e.id_equipos == id);

            //Verificamos que existe el registro segun su id

            if (equipo == null)
            {
                return NotFound();
            }

            //ejecutamos la accion de eliminar el registro
            _equiposContexto.equipos.Attach(equipo);
            _equiposContexto.equipos.Remove(equipo);
            _equiposContexto.SaveChanges();
            return Ok(equipo);
        }

        }
}

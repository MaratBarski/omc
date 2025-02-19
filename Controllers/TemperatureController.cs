using Microsoft.AspNetCore.Mvc;
using OMC.DTOs;
using OMC.Models;
using OMC.Services;

namespace OMC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TemperatureController : ControllerBase
    {
        private readonly TemperatureManager _temperatureManager;

        public TemperatureController(TemperatureManager temperatureManager)
        {
            _temperatureManager = temperatureManager;
        }


        [HttpGet("malfunctions")]
        public ActionResult GetMalfunctions()
        {
            try
            {
                return Ok(new { malfunctions = _temperatureManager.GetMalfunctions() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("last-week")]
        public ActionResult AggrigateLastWeek()
        {
            try
            {
                return Ok(_temperatureManager.AggrigateLastWeek());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("removed")]
        public ActionResult GetRemovedSensors()
        {
            try
            {
                return Ok(_temperatureManager.GetRemovedSensors());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult AddSensor([FromBody] AddSensorDto addSensorDto)
        {
            try
            {
                _temperatureManager.AddSensor(addSensorDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{face}/{sensorId}")]
        public ActionResult DeleteSensor([FromRoute] FaceSide face, [FromRoute] int sensorId)
        {
            try
            {
                _temperatureManager.RemoveSensor(face, sensorId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

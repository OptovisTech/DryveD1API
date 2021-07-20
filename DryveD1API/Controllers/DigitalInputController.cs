using System;
using DryveD1API.Common;
using DryveD1API.Modules;
using Microsoft.AspNetCore.Mvc;

namespace DryveD1API.Controllers
{
    /// <summary>
    ///  Read and write digital inputs.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DigitalInputController : ControllerBase
    {
        /// <summary>
        /// 60FD<br />
        /// Status display of digital inputs.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("{hostIp}/{port}")]
        public DigitalInputs Get(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var digitalInputs = new DigitalInputs();
            digitalInputs.Read(connection.socket);
            return digitalInputs;
        }

        /// <summary>
        /// 2010h<br />
        /// Activation of the input signal negation of digital inputs.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns></returns>
        [HttpGet("Polarity/{hostIp}/{port}")]
        public DigitalInputPolarity GetPolarity(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var digitalInputPolarity = new DigitalInputPolarity();
            digitalInputPolarity.Read(connection.socket);
            return digitalInputPolarity;
        }

        /// <summary>
        /// 2010h<br />
        /// Activation of the input signal negation of digital inputs.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="digitalInputPolarity"></param>
        [HttpPut("Polarity/{hostIp}/{port}")]
        public void SetPolarity(string hostIp, int port, [FromBody] DigitalInputPolarity digitalInputPolarity)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            digitalInputPolarity.Write(connection.socket);
        }

        /// <summary>
        /// 2010h<br />
        /// Toggle a specific digital input.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="digitalInput"></param>
        /// <returns></returns>
        [HttpPut("Toggle/{hostIp}/{port}/{digitalInput}")]
        public bool TogglePolarity(string hostIp, int port, int digitalInput)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var digitalInputPolarity = new DigitalInputPolarity();
            return digitalInputPolarity.ToggleDigitalInput(connection.socket, digitalInput);
        }
    }
}

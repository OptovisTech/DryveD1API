using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using DryveD1API.Common;
using DryveD1API.Modules;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DryveD1API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class InitController : ControllerBase
    {
        /// <summary>
        /// This method starts the initialization process
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns>True when initialization is finished</returns>
        [HttpGet("{hostIp}/{port:int}")]
        public bool Init(string hostIp, int port)
        {
            var connection = ModbusSocket.GetConnection(hostIp, port);
            var modesOfOperation = new ModesOfOperation();
            modesOfOperation.Write(connection.Socket, (ModesOfOperation.ModesEnum)1);

            while (modesOfOperation.ReadDisplay(connection.Socket) != ModesOfOperation.ModesEnum.ProfilePosition)
            {
                Thread.Sleep(10);
            }

            var status = new StatusWord();
            status.Read(connection.Socket);
            if (!status.Bit01 && !status.Bit02 && status.Bit06)
            {
                Reset(connection.Socket);
                ShutDown(connection.Socket);
                SwitchOn(connection.Socket);
            }

            EnableOperation(connection.Socket);
            return true;
        }

        /// <summary>
        /// This method starts the initialization process
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="cancellationToken"></param>
        /// <returns>True when initialization is finished</returns>
        [HttpGet("Async/{hostIp}/{port:int}")]
        public async Task<IActionResult> InitAsync(string hostIp, int port, CancellationToken cancellationToken)
        {
            try
            {
                var connection = ModbusSocket.GetConnection(hostIp, port);
                var modesOfOperation = new ModesOfOperation();
                await modesOfOperation.WriteAsync(connection.Socket, (ModesOfOperation.ModesEnum)1, cancellationToken);

                while (await modesOfOperation.ReadDisplayAsync(connection.Socket, cancellationToken) != ModesOfOperation.ModesEnum.ProfilePosition)
                {
                    await Task.Delay(10, cancellationToken);
                }

                var status = new StatusWord();
                await status.ReadAsync(connection.Socket, cancellationToken);
                if (!status.Bit01 && !status.Bit02 && status.Bit06)
                {
                    await ResetAsync(connection.Socket, cancellationToken);
                    await ShutDownAsync(connection.Socket, cancellationToken);
                    await SwitchOnAsync(connection.Socket, cancellationToken);
                }

                await EnableOperationAsync(connection.Socket, cancellationToken);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        private static void Reset(Socket s)
        {
            var controlWord = new ControlWord
            {
                // Byte 19: 6
                Bit01 = true // 2
            };
            controlWord.Write(s);
            Thread.Sleep(10);
        }

        private static async Task ResetAsync(Socket s, CancellationToken cancellationToken)
        {
            var controlWord = new ControlWord
            {
                // Byte 19: 6
                Bit01 = true // 2
            };
            await controlWord.WriteAsync(s, cancellationToken);
            await Task.Delay(10, cancellationToken);
        }

        private static void ShutDown(Socket s)
        {
            var controlWord = new ControlWord
            {
                // Byte 19: 6
                Bit01 = true, // 2
                Bit02 = true // 4
            };
            controlWord.Write(s);

            var statusWord = new StatusWord();
            while (!(statusWord.Bit00 && statusWord.Bit05 // 33
                                      && statusWord.Bit09)) // 2
            {
                statusWord.Read(s);
                Thread.Sleep(10);
            }
        }

        private static async Task ShutDownAsync(Socket s, CancellationToken cancellationToken)
        {
            var controlWord = new ControlWord
            {
                // Byte 19: 6
                Bit01 = true, // 2
                Bit02 = true // 4
            };
            await controlWord.WriteAsync(s, cancellationToken);

            var statusWord = new StatusWord();
            while (!(statusWord.Bit00 && statusWord.Bit05 // 33
                                      && statusWord.Bit09)) // 2
            {
                await statusWord.ReadAsync(s, cancellationToken);
                await Task.Delay(10, cancellationToken);
            }
        }

        private static void SwitchOn(Socket s)
        {
            var controlWord = new ControlWord
            {
                // Byte 19: 7
                Bit00 = true, // 1
                Bit01 = true, // 2
                Bit02 = true // 4
            };
            controlWord.Write(s);

            var statusWord = new StatusWord();
            while (!(statusWord.Bit00 && statusWord.Bit01 && statusWord.Bit05 // 35
                     && statusWord.Bit09)) // 2
            {
                statusWord.Read(s);
                Thread.Sleep(10);
            }
        }

        private static async Task SwitchOnAsync(Socket s, CancellationToken cancellationToken)
        {
            var controlWord = new ControlWord
            {
                // Byte 19: 7
                Bit00 = true, // 1
                Bit01 = true, // 2
                Bit02 = true // 4
            };
            await controlWord.WriteAsync(s, cancellationToken);

            var statusWord = new StatusWord();
            while (!(statusWord.Bit00 && statusWord.Bit01 && statusWord.Bit05 // 35
                     && statusWord.Bit09)) // 2
            {
                await statusWord.ReadAsync(s, cancellationToken);
                await Task.Delay(10, cancellationToken);
            }
        }

        private static void EnableOperation(Socket s)
        {
            var controlWord = new ControlWord
            {
                // Byte 19: 15
                Bit00 = true, // 1
                Bit01 = true, // 2
                Bit02 = true, // 4
                Bit03 = true // 8
            };
            controlWord.Write(s);

            var statusWord = new StatusWord();
            while (!(statusWord.Bit00 && statusWord.Bit01 && statusWord.Bit02 && statusWord.Bit05 // 39
                     && statusWord.Bit09)) // 2
            {
                statusWord.Read(s);
                Thread.Sleep(10);
            }
        }

        private static async Task EnableOperationAsync(Socket s, CancellationToken cancellationToken)
        {
            var controlWord = new ControlWord
            {
                // Byte 19: 15
                Bit00 = true, // 1
                Bit01 = true, // 2
                Bit02 = true, // 4
                Bit03 = true // 8
            };
            await controlWord.WriteAsync(s, cancellationToken);

            var statusWord = new StatusWord();
            while (!(statusWord.Bit00 && statusWord.Bit01 && statusWord.Bit02 && statusWord.Bit05 // 39
                     && statusWord.Bit09)) // 2
            {
                await statusWord.ReadAsync(s, cancellationToken);
                await Task.Delay(10, cancellationToken);
            }
        }
    }
}

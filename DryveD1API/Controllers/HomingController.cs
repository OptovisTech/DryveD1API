using System.Net.Sockets;
using System.Threading;
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
    public class HomingController : ControllerBase
    {

        /// <summary>
        /// This method starts the homing process
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <returns>True when homing is finished</returns>
        [HttpGet("{hostIp}/{port}")]
        public bool Homing(string hostIp, int port)
        {
            using (Socket s = ModbusSocket.Connect(hostIp, port))
            {
                ModesOfOperation modesOfOperation = new ModesOfOperation();
                modesOfOperation.Write(s, ModesOfOperation.ModesEnum.Homing);
                while (modesOfOperation.ReadDisplay(s) != ModesOfOperation.ModesEnum.Homing)
                {
                    Thread.Sleep(100);
                }
                ControlWord controlWord = new ControlWord();
                // Byte 19: 31
                controlWord.Bit00 = true; // 1
                controlWord.Bit01 = true; // 2
                controlWord.Bit02 = true; // 4
                controlWord.Bit03 = true; // 8
                controlWord.Bit04 = true; // 16
                controlWord.Write(s);

                StatusWord statusWord = new StatusWord();
                while (!(statusWord.Bit00 && statusWord.Bit01 && statusWord.Bit02 && statusWord.Bit05 // 39
                    && statusWord.Bit09 && statusWord.Bit10 && statusWord.Bit12)) // 22
                {
                    statusWord.Read(s);
                    Thread.Sleep(100);
                }
                ModbusSocket.Close(s);
                return true;
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using DryveD1API.Common;
using DryveD1API.Modules;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DryveD1API.Controllers
{
    /// <summary>
    /// Execute common Instructions
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class InstructionController : Controller
    {
        /// <summary>
        /// 607Ah<br />
        /// Set Target Position, start and wait until PositionReached.
        /// </summary>
        /// <param name="hostIp">Ip Address of the Dryve D1 Controller</param>
        /// <param name="port">Port of the Dryve D1 Controller</param>
        /// <param name="instruction">Contains the values to be set</param>
        /// <returns></returns>
        [HttpPost("MoveToTarget/{hostIp}/{port}")]
        public async Task<bool> MoveToTarget(string hostIp, int port, [FromBody] Instruction instruction)
        {
            return await instruction.MoveToTarget(hostIp, port);
        }


        private void PrintStatusWord(Socket s)
        {
            StatusWord sw = new StatusWord();
            sw.Read(s);
            var bitArray19 = new BitArray(new bool[8] { sw.Bit00, sw.Bit01, sw.Bit02, sw.Bit03, sw.Bit04, sw.Bit05, sw.Bit06, sw.Bit07 });
            var bitArray20 = new BitArray(new bool[8] { sw.Bit08, sw.Bit09, sw.Bit10, sw.Bit11, sw.Bit12, sw.Bit13, sw.Bit14, sw.Bit15 });

            byte[] byte19 = new byte[1];
            bitArray19.CopyTo(byte19, 0);

            byte[] byte20 = new byte[1];
            bitArray20.CopyTo(byte20, 0);
            Debug.WriteLine("");
            Debug.WriteLine("StatusWord");
            Debug.WriteLine($"Byte 19: {byte19[0]}");
            Debug.WriteLine($"0: {sw.Bit00}, 1: {sw.Bit01}, 2: {sw.Bit02}, 3: {sw.Bit03}, 4: {sw.Bit04}, 5: {sw.Bit05}, 6: {sw.Bit06}, 7: {sw.Bit07}");
            Debug.WriteLine($"Byte 20: {byte20[0]}");
            Debug.WriteLine($"8: {sw.Bit08}, 9: {sw.Bit09}, 10: {sw.Bit10}, 11: {sw.Bit11}, 12: {sw.Bit12}, 13: {sw.Bit13}, 14: {sw.Bit14}, 15: {sw.Bit15}");
            Debug.WriteLine("");
        }

        private void PrintControlWord(Socket s)
        {
            ControlWord cw = new ControlWord();
            cw.Read(s);
            var bitArray19 = new BitArray(new bool[8] { cw.Bit00, cw.Bit01, cw.Bit02, cw.Bit03, cw.Bit04, cw.Bit05, cw.Bit06, cw.Bit07 });
            var bitArray20 = new BitArray(new bool[8] { cw.Bit08, cw.Bit09, cw.Bit10, cw.Bit11, cw.Bit12, cw.Bit13, cw.Bit14, cw.Bit15 });

            byte[] byte19 = new byte[1];
            bitArray19.CopyTo(byte19, 0);

            byte[] byte20 = new byte[1];
            bitArray20.CopyTo(byte20, 0);
            Debug.WriteLine("");
            Debug.WriteLine("ControlWord");
            Debug.WriteLine($"Byte 19: {byte19[0]}");
            Debug.WriteLine($"0: {cw.Bit00}, 1: {cw.Bit01}, 2: {cw.Bit02}, 3: {cw.Bit03}, 4: {cw.Bit04}, 5: {cw.Bit05}, 6: {cw.Bit06}, 7: {cw.Bit07}");
            Debug.WriteLine($"Byte 20: {byte20[0]}");
            Debug.WriteLine($"8: {cw.Bit08}, 9: {cw.Bit09}, 10: {cw.Bit10}, 11: {cw.Bit11}, 12: {cw.Bit12}, 13: {cw.Bit13}, 14: {cw.Bit14}, 15: {cw.Bit15}");
            Debug.WriteLine("");
        }
    }
}

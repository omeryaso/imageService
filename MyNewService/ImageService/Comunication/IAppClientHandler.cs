using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Comunication
{
    interface IAppClientHandler
    {
        /// <summary>
        /// handle the client
        /// </summary>
        /// <param name="client"></param> the tcpClient
        /// <param name="clients"></param> the list of clients
        void HandleClient(TcpClient client, List<TcpClient> clients);
    }
}

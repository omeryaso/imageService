using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Comunication
{
    class AppClientHandler : IAppClientHandler
    {
        public void HandleClient(TcpClient client, List<TcpClient> clients)
        {
            throw new NotImplementedException();
        }
    }
}

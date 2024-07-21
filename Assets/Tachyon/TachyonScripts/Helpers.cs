using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tachyon
{
    class VRClient
    {
        private string clientID;
        private ClientType clientType;
        private string moduleName;

        public VRClient(string clientID)
        {
            this.clientID = clientID;
        }

        public void setClientType(ClientType clientType)
        {
            this.clientType = clientType;
        }

        public string getClientID()
        {
            return clientID;
        }

        public ClientType getClientType()
        {
            return clientType;
        }

        public void setModuleName(string moduleName)
        {
            this.moduleName = moduleName;
        }

        public string getModuleName()
        {
            return moduleName;
        }
    }

    enum ClientType
    {
        Desktop,
        Headset
    }
}

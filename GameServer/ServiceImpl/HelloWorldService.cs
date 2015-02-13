using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;


namespace SpaceTraffic.GameServer.ServiceImpl
{
    [ServiceContract]
    public interface IHelloWorldService
    {
        [OperationContract]
        string SayHello(string name);
    }

    public class HelloWorldService : IHelloWorldService
    {

        public string SayHello(string name)
        {
            return string.Format("Hello, {0}", name);
        }
    }
}

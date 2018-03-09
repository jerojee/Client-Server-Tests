using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using ProtoBuf;
using ChitChat.Events;

namespace csharp_server
{
    class Server
    {

        void PrintUserInfo(UserInfo user)
        {
            Console.WriteLine($"Name: {user.Name}");
            Console.WriteLine($"ID: {user.ID}");
            Console.WriteLine($"Password: {user.Password}");
            Console.WriteLine($"Event Type: {user.Type.ToString()}");
        }

        public void StartServer()
        {
            Console.WriteLine("Starting server....");

            TcpListener listener = null;

            try
            {
                IPAddress localIP = IPAddress.Any;
                int port = 27015;

                listener = new TcpListener(localIP, port);

                listener.Start();

                BaseEvent incomingEvent = new BaseEvent
                {
                    Type = EventType.NONE
                };

                int count = 0;
                //testing 2 clients
                TcpClient newClient1 = null;
                TcpClient newClient2 = null;
                List<TcpClient> listConnectedClients = new List<TcpClient>();

           
                while (count < 2)
                {
                    Console.WriteLine("Waiting for connection....\n");

                     newClient1 = listener.AcceptTcpClient();
                    newClient2 = listener.AcceptTcpClient();

                     Console.WriteLine("Accepted a connection! \n");

  
                }

                while (true)
                {
                    //List<TcpClient> listConnectedClients = new List<TcpClient>(); //List holds connected clients 
                    
                        //Console.WriteLine("Waiting for connection....\n");

                        // newClient = listener.AcceptTcpClient();

                      //  Console.WriteLine("Accepted a connection! \n");

                    //    listConnectedClients.Add(newClient); //Add new client to List of connected clients

                    //BaseEvent incomingEvent = new BaseEvent();

                    if (!(incomingEvent.Type == EventType.NONE))
                    {
                        using (var stream = newClient.GetStream())
                        {
                            incomingEvent = Serializer.DeserializeWithLengthPrefix<BaseEvent>(stream, PrefixStyle.Base128);
                        }
                    }

                    switch (incomingEvent.Type)
                    {
                        case EventType.NONE:
                            Console.WriteLine("No event yet");
                            break;

                        case EventType.USER_INFO:
                            UserInfo user = (UserInfo)incomingEvent;
                            Console.WriteLine("Event was of type: {0}", incomingEvent.Type.ToString());
                            PrintUserInfo(user);
                            break;

                        case EventType.SEND_MESSAGE:
                            Console.WriteLine("Recieved SEND MESSAGE request...");
                            Console.WriteLine("Event was of type: {0}", incomingEvent.Type.ToString());
                            SendMessage m = (SendMessage)incomingEvent;
                            string s;
                            s = m.Message; //store message from client into string
                            SendMessage newMessage = new SendMessage
                            {
                                Message = s
                            };
                            using (var stream = newClient.GetStream())
                            {
                                foreach (TcpClient client in listConnectedClients) //send Message to each connected client
                                {
                                    
                                    Serializer.SerializeWithLengthPrefix(stream, newMessage, PrefixStyle.Base128);
                                }
                            }
                            Console.WriteLine("SEND MESSAGE REQUEST HANDLED");
                            Console.WriteLine("Event was of type: {0}", incomingEvent.Type.ToString());
                            break;

                        default:
                            Console.WriteLine("Type was not known.");
                            break;
                    }

  
     
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        static void Main(string[] args)
        {
            Server s = new Server { };

            s.StartServer();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using ProtoBuf;
using ChitChat.Events;

namespace Test_Client1
{
    class Client
    {
        void PrintUserInfo(UserInfo user, Login userLogin)
        {
            Console.WriteLine($"Name: {user.Name}");
            Console.WriteLine($"ID: {user.ID}");
            Console.WriteLine($"Password: {user.Password}");
            Console.WriteLine($"Event Type: {user.Type.ToString()}");

            Console.WriteLine($"User: {userLogin.Username}");
            Console.WriteLine($"User: {userLogin.Password}");
        }

       void PrintMessageToClient(SendMessage m)
        {
            Console.WriteLine($"Recieved Message: {m.Message}");
        }

        public void StartClient()
        {
            Console.WriteLine("Starting Client....");

            UserInfo user = new UserInfo
            {
                Name = "Jeremy",
                ID = 33690,
                PhoneNumber = "111-111-1111",
                Password = "Password",
                Type = EventType.USER_INFO,
            };

            Login userLogin = new Login
            {
                Username = "xkrunchy",
                Password = "Mastersword!!64",
                Type = EventType.LOGIN,
            };


            Console.WriteLine("Create new user with info: \n");
            PrintUserInfo(user, userLogin);

            TcpClient _client = null;
            NetworkStream stream;
            
            try
            {

                _client = new TcpClient("144.37.220.39", 27015);

                stream = _client.GetStream();

                string answer;
                string newMessageToBeSent;

                BaseEvent incomingEvent = new BaseEvent
                {
                    Type = EventType.NONE
                };

                while (true)
                {                    
                    //Sending a new message code below
                    Console.WriteLine("Do you want to send a message? yes/no?");
                    answer = Console.ReadLine();
                    if (answer == "yes")
                    {
                        Console.WriteLine("Enter message to be sent: ");
                        newMessageToBeSent = Console.ReadLine();
                        SendMessage message = new SendMessage
                        {
                            Message = newMessageToBeSent,
                            Type = EventType.SEND_MESSAGE
                        };
                                                   
                            Serializer.SerializeWithLengthPrefix(stream, message, PrefixStyle.Base128);
                            Console.WriteLine("Sent a {0} event to server...", message.Type.ToString());                          
                        

                        Console.ReadKey();
                    }
                                                                           
                       incomingEvent = Serializer.DeserializeWithLengthPrefix<BaseEvent>(stream, PrefixStyle.Base128);   
                                          
                        switch (incomingEvent.Type)
                        {
                            case EventType.NEW_USER: 
                                break;

                            case EventType.LOGIN:
                                Login l = (Login)incomingEvent;
                                Console.WriteLine("Event was of type: {0}", incomingEvent.Type.ToString());
                                break;

                            case EventType.USER_INFO:
                                break;

                            case EventType.CREATE_CHAT:
                                break;
                           
                            case EventType.SEND_MESSAGE:
                                SendMessage m = (SendMessage)incomingEvent;
                                Console.WriteLine("Event was of type: {0}", incomingEvent.Type.ToString());
                                PrintMessageToClient(m);
                                break;

                            case EventType.NONE:
                                Console.WriteLine("No event yet");
                                break;

                            default:
                                break;
                        }                   
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Socket exception: {0}", e.ToString());
            }
            finally
            {
                _client?.Close();
            }
        }


        static int Main(String[] args)
        {
            Client c = new Client { };

            c.StartClient();

            return (0);
        }
    }
}


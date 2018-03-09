using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace ChitChat.Events
{
    public enum EventType
    {
        NEW_USER,
        LOGIN,
        USER_INFO,
        CREATE_CHAT, //JEREMY: Adding new types
        SEND_MESSAGE, // new
        NONE,
    }

    [ProtoContract]
    [ProtoInclude(100, typeof(UserInfo))]
    [ProtoInclude(101, typeof(Login))]
    [ProtoInclude(102, typeof(NewUser))]
    [ProtoInclude(103, typeof(CreateChat))]
    [ProtoInclude(104, typeof(SendMessage))]
    class BaseEvent
    {
        [ProtoMember(1)]
        public EventType Type { get; set; }
    }

    [ProtoContract]
    class UserInfo : BaseEvent
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public int ID { get; set; }

        [ProtoMember(3)]
        public string PhoneNumber { get; set; }

        [ProtoMember(4)]
        public string Password { get; set; }
    }

    //JEREMY: Adding proto structs
    [ProtoContract]
    class Login : BaseEvent
    {
        [ProtoMember(1)]
        public string Username { get; set; }

        [ProtoMember(2)]
        public string Password { get; set; }
    }

    [ProtoContract]
    class NewUser : BaseEvent
    {
        [ProtoMember(1)]
        public string FirstName { get; set; }

        [ProtoMember(2)]
        public string LastName { get; set; }

        [ProtoMember(3)]
        public string PhoneNumber { get; set; }

        [ProtoMember(4)]
        public string Password { get; set; }
    }

    [ProtoContract]
    class CreateChat : BaseEvent
    {
        [ProtoMember(1)]
        public string ChatName { get; set; }

        //Possibly add member for a list of users?
    }

    [ProtoContract]
    class SendMessage : BaseEvent
    {
        [ProtoMember(1)]
        public string Message { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamNotification_Library.Models
{
    public class ChatRoomInvitation
    {
        public string chat_room_name { get; set; }
        public string chat_room_id { get; set; }
        public User user { get; set; }
        public string invited_user_id { get; set; }
    }
}

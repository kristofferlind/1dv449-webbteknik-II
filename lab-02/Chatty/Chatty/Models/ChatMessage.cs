using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chatty.Models
{
    public class ChatMessage
    {
        [BsonId]
        public string Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime Date { get; set; }
        
        public ChatMessage(string name, string message)
        {
            Name = name;
            Message = message;
            Date = DateTime.Now;
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Projise.DomainModel.ChattyBrightStarModel
{
    class ChatMessage_Metadata
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}

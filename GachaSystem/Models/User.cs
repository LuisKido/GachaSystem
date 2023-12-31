﻿using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GachaSystem.Models
{
    public class User
    {
        [DynamoDBHashKey] // Esto denota que este atributo es tu clave de partición
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int TotalScore { get; set; }

        public int GachaTickets { get; set; }

        public Dictionary<string, int> Characters { get; set; } = new Dictionary<string, int>();
    }
}

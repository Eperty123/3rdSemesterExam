﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class TokenDTO
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string UserType { get; set; }
    }
}

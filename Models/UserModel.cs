﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Final_Property_Rental.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
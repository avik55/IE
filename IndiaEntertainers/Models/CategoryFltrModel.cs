﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndiaEntertainers.Models
{
    public class CategoryFltrModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int CatId { get; set; }
        public string URL { get; set; }
    }
}
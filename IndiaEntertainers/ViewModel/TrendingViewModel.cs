using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndiaEntertainers.ViewModel
{
    public class TrendingViewModel
    {
        public long EntrId { get; set; }

        public decimal PerformanceFees { get; set; }

        public string CityName { get; set; }

        public string ProfilePhoto { get; set; }

        public string Type { get; set; }

        public string FullName { get; set; }

        public string CategoryName { get; set; }

        public bool Showfee { get; set; }

        public string Slug { get; set; }

    }
}
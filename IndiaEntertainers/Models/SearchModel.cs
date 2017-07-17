using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndiaEntertainers.Models
{
    public class SearchModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long EntrId { get; set; }
        public string Title { get; set; }
        public bool IsItCat { get; set; }
        public string URL { get; set; }
        public string City { get; set; }
        public string Category { get; set; }
        public decimal fee { get; set; }
        public string imgurl { get; set; }
        public int CityId { get; set; }
        public int CategoryId { get; set; }
        public int GenderId { get; set; }
        public int NationalityId { get; set; }
        public int PerfLangId { get; set; }
        public bool Showfee { get; set; }
    
    }
}
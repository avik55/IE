using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndiaEntertainers.Models
{
    public class FilterModel
    {
        public long Id { get; set; }
        public decimal FlbudgetMin { get; set; }
        public decimal FlbudgetMax { get; set; }
        public List<int> FlGender { get; set; }
        public List<int> FlNationality { get; set; }
        public List<int> FlPerfLanguage { get; set; }
        public List<int> FlCategory { get; set; }
      //  public List<int> FlCity { get; set; }
        public List<CityFltrModel> FlCity { get; set; }
        public int PageNo { get; set; }
    }

    public class FLCityModel
    {
        public int Id { get; set; }
        public string CityName { get; set; }
    }
}
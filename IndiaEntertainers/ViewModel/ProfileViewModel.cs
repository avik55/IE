using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndiaEntertainers.ViewModel
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        public long EntrId { get; set; }
        public string FName { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public int? NoOfShow { get; set; }
        public string PerfLang { get; set; }
        public int PerfLength { get; set; }
        public string PerfMembers { get; set; }
        public string ProfTitle { get; set; }
        public string ProfDetail { get; set; }
        public string FbLink { get; set; }
        public string InstLink { get; set; }
        public string TwtLink { get; set; }
        public decimal PerfFee { get; set; }
        public int? CatId { get; set; }
        public int? CityId { get; set; }
        public int CatTitle { get; set; }
        public string City { get; set; }
        public bool Showfee { get; set; }
        public bool DonotShowfee { get; set; }
        public bool PerfLang1 { get; set; }
        public bool PerfLang2 { get; set; }
        public string PerfLangTitle { get; set; }
        public string Mobile { get; set; }

        // public List<int> PerfLanges { get; set; }
    }

    public class CityModel
    {
        public int CityId { get; set; }
        public int CityName { get; set; }
    }
}
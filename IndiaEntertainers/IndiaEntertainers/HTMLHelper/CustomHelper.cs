using IndiaEntertainers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace IndiaEntertainers.HTMLHelper
{
    public class CustomHelper 
    {
        private static IEDBContext db = new IEDBContext();

        public static IHtmlString StateDropDownList(string name, string ClassName, string id, string selectdvalue)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("<select name='{0}'  class='{1}' id='{2}' required><option value=''>-- Select State --</option>", name, ClassName, id);
            if (String.IsNullOrEmpty(selectdvalue))
            {
                var states = db.tbl_State.OrderBy(f => f.StateName).ToList();
                foreach (var st in states)
                {
                    SB.AppendFormat("<option value='{0}'>{1}</option>", st.StateId, st.StateName);
                }
            }
            else
            {
                var states = db.tbl_State.OrderBy(f => f.StateName).ToList();
                foreach (var st in states)
                {
                    if (!String.IsNullOrWhiteSpace(selectdvalue) && selectdvalue == st.StateId.ToString())
                        SB.AppendFormat("<option value='{0}' selected>{1}</option>", st.StateId, st.StateName);
                    else
                        SB.AppendFormat("<option value='{0}'>{1}</option>", st.StateId, st.StateName);
                }
            }

            //SB.AppendFormat("<option value='Deleted' >Deleted</option>");
            SB.Append("</select>");
            return new HtmlString(SB.ToString());
        }

        public static IHtmlString CityDropDownList(string name, string ClassName, string id, string selectdvalue)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("<select name='{0}' class='{1}' id='{2}'><option value=''>-- Select City --</option>", name, ClassName, id);
            if (String.IsNullOrEmpty(selectdvalue))
            {
                var cities = db.tbl_City.OrderBy(f => f.CityName).ToList();
                foreach (var st in cities)
                {
                    SB.AppendFormat("<option value='{0}'>{1}</option>", st.CityId, st.CityName);
                }
            }
            else
            {
                var cities = db.tbl_City.OrderBy(f => f.CityName).ToList();
                foreach (var st in cities)
                {
                    if (!String.IsNullOrWhiteSpace(selectdvalue) && selectdvalue == st.CityId.ToString())
                        SB.AppendFormat("<option value='{0}' selected>{1}</option>", st.CityId, st.CityName);
                    else
                        SB.AppendFormat("<option value='{0}'>{1}</option>", st.CityId, st.CityName);
                }
            }

            //SB.AppendFormat("<option value='Deleted' >Deleted</option>");
            SB.Append("</select>");
            return new HtmlString(SB.ToString());
        }

        public static IHtmlString GenderDropDownList(string name, string ClassName, string id, string selectdvalue)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("<select name='{0}' class='{1}' id='{2}' ><option value=''>-- Select Gender --</option>", name, ClassName, id);
            if (String.IsNullOrEmpty(selectdvalue))
            {
                SB.AppendFormat("<option value='Male' >Male</option>");
                SB.AppendFormat("<option value='Female' >Female</option>");
                SB.AppendFormat("<option value='All Male group' >All Male group</option>");
                SB.AppendFormat("<option value='All Female Group' >All Female Group</option>");
                SB.AppendFormat("<option value='Mixed Group' >Mixed Group</option>");


            }
            else
            {
                if (selectdvalue == "Male")
                {
                    SB.AppendFormat("<option value='Male' selected>Male</option>");
                    SB.AppendFormat("<option value='Female'>Female</option>");
                    SB.AppendFormat("<option value='All Male group' >All Male group</option>");
                    SB.AppendFormat("<option value='All Female Group' >All Female Group</option>");
                    SB.AppendFormat("<option value='Mixed Group' >Mixed Group</option>");
                }
                else if (selectdvalue == "Female")
                {
                    SB.AppendFormat("<option value='Male'>Male</option>");
                    SB.AppendFormat("<option value='Female' selected>Female</option>");
                    SB.AppendFormat("<option value='All Male group' >All Male group</option>");
                    SB.AppendFormat("<option value='All Female Group' >All Female Group</option>");
                    SB.AppendFormat("<option value='Mixed Group' >Mixed Group</option>");
                }
                else if (selectdvalue == "All Male group")
                {
                    SB.AppendFormat("<option value='Male' selected>Male</option>");
                    SB.AppendFormat("<option value='Female'>Female</option>");
                    SB.AppendFormat("<option value='All Male group' selected>All Male group</option>");
                    SB.AppendFormat("<option value='All Female Group' >All Female Group</option>");
                    SB.AppendFormat("<option value='Mixed Group' >Mixed Group</option>");
                }
                else if (selectdvalue == "All Male group")
                {
                    SB.AppendFormat("<option value='Male' selected>Male</option>");
                    SB.AppendFormat("<option value='Female'>Female</option>");
                    SB.AppendFormat("<option value='All Male group' >All Male group</option>");
                    SB.AppendFormat("<option value='All Female Group' selected>All Female Group</option>");
                    SB.AppendFormat("<option value='Mixed Group' >Mixed Group</option>");
                }
                else if (selectdvalue == "Mixed Group")
                {
                    SB.AppendFormat("<option value='Male' >Male</option>");
                    SB.AppendFormat("<option value='Female'>Female</option>");
                    SB.AppendFormat("<option value='All Male group' selected>All Male group</option>");
                    SB.AppendFormat("<option value='All Female Group' >All Female Group</option>");
                    SB.AppendFormat("<option value='Mixed Group' selected>Mixed Group</option>");
                }
            }

            //SB.AppendFormat("<option value='Deleted' >Deleted</option>");
            SB.Append("</select>");
            return new HtmlString(SB.ToString());
        }

        public static IHtmlString StarDropDownList(string name, string ClassName, string id, string selectdvalue)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("<select name='{0}' class='{1}' id='{2}' ><option value=''>-- Select Star --</option>", name, ClassName, id);
            if (String.IsNullOrEmpty(selectdvalue))
            {
                SB.AppendFormat("<option value='1' >1</option>");
                SB.AppendFormat("<option value='2' >2</option>");
                SB.AppendFormat("<option value='3' >3</option>");
                SB.AppendFormat("<option value='4' >4</option>");
                SB.AppendFormat("<option value='5' >5</option>");
            }
            else
            {
                if (selectdvalue == "1")
                {
                    SB.AppendFormat("<option value='1' selected>1</option>");
                    SB.AppendFormat("<option value='2' >2</option>");
                    SB.AppendFormat("<option value='3' >3</option>");
                    SB.AppendFormat("<option value='4' >4</option>");
                    SB.AppendFormat("<option value='5' >5</option>");
                }
                else if (selectdvalue == "2")
                {
                    SB.AppendFormat("<option value='1' >1</option>");
                    SB.AppendFormat("<option value='2' selected>2</option>");
                    SB.AppendFormat("<option value='3' >3</option>");
                    SB.AppendFormat("<option value='4' >4</option>");
                    SB.AppendFormat("<option value='5' >5</option>");
                }
                else if (selectdvalue == "3")
                {
                    SB.AppendFormat("<option value='1' >1</option>");
                    SB.AppendFormat("<option value='2' >2</option>");
                    SB.AppendFormat("<option value='3' selected>3</option>");
                    SB.AppendFormat("<option value='4' >4</option>");
                    SB.AppendFormat("<option value='5' >5</option>");
                }
                else if (selectdvalue == "4")
                {
                    SB.AppendFormat("<option value='1' >1</option>");
                    SB.AppendFormat("<option value='2' >2</option>");
                    SB.AppendFormat("<option value='3' >3</option>");
                    SB.AppendFormat("<option value='4' selected>4</option>");
                    SB.AppendFormat("<option value='5' >5</option>");
                }
                else if (selectdvalue == "5")
                {
                    SB.AppendFormat("<option value='1' >1</option>");
                    SB.AppendFormat("<option value='2' >2</option>");
                    SB.AppendFormat("<option value='3' >3</option>");
                    SB.AppendFormat("<option value='4' >4</option>");
                    SB.AppendFormat("<option value='5' selected>5</option>");
                }
                else
                {
                    SB.AppendFormat("<option value='1' >1</option>");
                    SB.AppendFormat("<option value='2' >2</option>");
                    SB.AppendFormat("<option value='3' >3</option>");
                    SB.AppendFormat("<option value='4' >4</option>");
                    SB.AppendFormat("<option value='5' >5</option>");
                }
            }

            //SB.AppendFormat("<option value='Deleted' >Deleted</option>");
            SB.Append("</select>");
            return new HtmlString(SB.ToString());
        }

        public static IHtmlString TypeDropDownList(string name, string ClassName, string id, string selectdvalue)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("<select name='{0}' class='{1}' id='{2}' ><option value=''>-- Select Type --</option>", name, ClassName, id);
            if (String.IsNullOrEmpty(selectdvalue))
            {
                SB.AppendFormat("<option value='Single' >Single</option>");
                SB.AppendFormat("<option value='Mixed Group' >Mixed Group</option>");
            }
            else
            {
                if (selectdvalue == "Single")
                {
                    SB.AppendFormat("<option value='Single' selected>Single</option>");
                    SB.AppendFormat("<option value='Mixed Group' >Mixed Group</option>");
                }
                else if (selectdvalue == "Mixed Group")
                {
                    SB.AppendFormat("<option value='Single' >Single</option>");
                    SB.AppendFormat("<option value='Mixed Group' selected>Mixed Group</option>");
                }
                else
                {
                    SB.AppendFormat("<option value='Single' >Single</option>");
                    SB.AppendFormat("<option value='Mixed Group' >Mixed Group</option>");
                }
            }

            //SB.AppendFormat("<option value='Deleted' >Deleted</option>");
            SB.Append("</select>");
            return new HtmlString(SB.ToString());
        }

        public static IHtmlString ReqAsDropDownList(string name, string ClassName, string id, string selectdvalue, string req)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("<select ng-model='{0}' ng-disabled='fielddisabler' class='{1}' id='{2}'  {3}><option value=''>-- Select --</option>", name, ClassName, id, req);
            if (String.IsNullOrEmpty(selectdvalue))
            {
                SB.AppendFormat("<option value='Entertainer' >Entertainer</option>");
                SB.AppendFormat("<option value='Talent Seeker' >Talent Seeker</option>");
            }
            else
            {
                if (selectdvalue == "Entertainer")
                {
                    SB.AppendFormat("<option value='Entertainer' selected>Entertainer</option>");
                    SB.AppendFormat("<option value='Talent Seeker' >Talent Seeker</option>");
                }
                else if (selectdvalue == "Talent Seeker")
                {
                    SB.AppendFormat("<option value='Entertainer' >Entertainer</option>");
                    SB.AppendFormat("<option value='Talent Seeker' selected>Talent Seeker</option>");
                }
                else
                {
                    SB.AppendFormat("<option value='Single' >Single</option>");
                    SB.AppendFormat("<option value='Mixed Group' >Mixed Group</option>");
                }
            }

            //SB.AppendFormat("<option value='Deleted' >Deleted</option>");
            SB.Append("</select>");
            return new HtmlString(SB.ToString());
        }

        public static IHtmlString CategoryDropDownList(string name, string ClassName, string id, string selectdvalue)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("<select name='{0}' class='{1}' id='{2}' ><option value=''>-- Select Category --</option>", name, ClassName, id);
            if (String.IsNullOrEmpty(selectdvalue))
            {
                var cats = db.tbl_Category.OrderBy(f => f.Title).ToList();
                foreach (var ct in cats)
                {
                    SB.AppendFormat("<option value='{0}'>{1}</option>", ct.CatId, ct.Title);
                }
            }
            else
            {
                var cats = db.tbl_Category.OrderBy(f => f.Title).ToList();
                foreach (var ct in cats)
                {
                    if (!String.IsNullOrWhiteSpace(selectdvalue) && selectdvalue == ct.CatId.ToString())
                        SB.AppendFormat("<option value='{0}' selected>{1}</option>", ct.CatId, ct.Title);
                    else
                        SB.AppendFormat("<option value='{0}'>{1}</option>", ct.CatId, ct.Title);
                }
            }

            //SB.AppendFormat("<option value='Deleted' >Deleted</option>");
            SB.Append("</select>");
            return new HtmlString(SB.ToString());
        }

        public static string GetStateName(int? id)
        {
            if (id == null)
                return "-";
            var state = db.tbl_State.Find(id);
            if (state == null)
                return "-";
            else
                return state.StateName;
        }

        public static string GetCityName(int? id)
        {
            if (id == null)
                return "-";
            var city = db.tbl_City.Find(id);
            if (city == null)
                return "-";
            else
                return city.CityName;
        }

        public static string GetEntCatName(long? id)
        {
            if (id == null)
                return "-";
            var cat = db.tbl_EntertainerCats.Where(d => d.EntrId == id).FirstOrDefault();
            if (cat == null)
                return "-";
            else
                return cat.tbl_Category.Title;
        }

        public static string GetYouTubeVideoImage(string url)
        {
            if (url == null)
                return "-";
            var cat = url.Split('/').LastOrDefault();
            if (cat == null)
                return "-";
            else
                return string.Format("https://img.youtube.com/vi/{0}/0.jpg", cat);
        }

        public static IHtmlString CategoryModelDropDownList(string selectdvalue)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("<button data-toggle='dropdown' class='btn dropdown-toggle' name='CatId' id='CatId' data-placeholder='Please select' required>");
            SB.Append("<span class='selected_catt'>Select Category Name</span> <span class='caret'></span></button><ul class='dropdown-menu' style='max-height:250px;'>");
            if (String.IsNullOrEmpty(selectdvalue))
            {
                var cats = db.tbl_Category.OrderBy(f => f.Title).ToList();
                foreach (var ct in cats)
                {
                    SB.Append("<li><div class='radio radio-danger''>");
                    SB.AppendFormat("<input type='radio' name='CatId' id='Cat-{0}' value='{1}'>", ct.CatId, ct.CatId);
                    SB.AppendFormat("<label for='Cat-{0}'>", ct.CatId);
                    SB.Append(ct.Title);
                    SB.Append("</label></div> </li>");
                }
            }
            else
            {
                var cats = db.tbl_Category.OrderBy(f => f.Title).ToList();
                foreach (var ct in cats)
                {
                    SB.Append("<li><div class='radio radio-danger''>");
                    SB.AppendFormat("<input type='radio' name='CatId' id='Cat-{0}' value='{1}'>", ct.CatId, ct.CatId);
                    SB.AppendFormat("<label for='Cat-{0}'>", ct.CatId);
                    SB.Append(ct.Title);
                    SB.Append("</label></div> </li>");
                }
            }
            SB.Append("</ul>");
            return new HtmlString(SB.ToString());
        }

        public static IHtmlString StateModelDropDownList(string selectdvalue)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("<button  data-toggle='dropdown' class='btn dropdown-toggle' name='StateId' id='StateId' data-placeholder='Please select' required>");
            SB.Append("<span class='selected_state'>Select State</span> <span class='caret'></span></button><ul class='dropdown-menu' style='max-height:250px;'>");
            var states = db.tbl_State.OrderBy(f => f.StateName).ToList();
            if (String.IsNullOrEmpty(selectdvalue))
            {
                foreach (var st in states)
                {
                    SB.Append("<li> <div class='radio radio-danger'>");
                    SB.AppendFormat("<input type='radio' name='StateId' id='St-{0}' value='{1}'>", st.StateId,st.StateId);
                    SB.AppendFormat("<label for='St-{0}'>",st.StateId);
                    SB.Append(st.StateName);
                    SB.Append("</label></div> </li>");
                }
            }
            else
            {
                foreach (var st in states)
                {
                    SB.Append("<li> <div class='radio radio - danger'>");
                    SB.AppendFormat("<input type='radio' name='StateId' id='St-{0}' value='{1}'>", st.StateId, st.StateId);
                    SB.AppendFormat("<label for='St-{0}'>", st.StateId);
                    SB.Append(st.StateName);
                    SB.Append("</label></div> </li>");
                }
            }
            SB.Append("</ul>");

            return new HtmlString(SB.ToString());
        }
    }

    public class CustomHLPRController : Controller
    {
        private static IEDBContext db = new IEDBContext();

        [HttpGet]
        public JsonResult GetCities(int StateId)
        {
            var cities = db.tbl_City.Where(d => d.StateId == StateId).OrderBy(f => f.CityName).ToList();
            var citieslist = cities.Select(x => new SelectListItem { Text = x.CityName, Value = x.CityId.ToString() });
            return Json(citieslist, JsonRequestBehavior.AllowGet);
        }
    }
}
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

        //State Dropdown List Helper
        public static IHtmlString StateDropDownList(string name, string ClassName, string id, string selectdvalue)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("<select name='{0}'  class='{1}' id='{2}' required ><option value=''>-- Select State --</option>", name, ClassName, id);
            if (String.IsNullOrEmpty(selectdvalue))
            {
                var states = db.tbl_State.OrderBy(f => f.StateName).ToList();
                foreach (var st in states)
                    SB.AppendFormat("<option value='{0}'>{1}</option>", st.StateId, st.StateName);
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

        //City Dropdown List Helper
        public static IHtmlString CityDropDownList(string name, string ClassName, string id, string selectdvalue, bool IsRegister)
        {
            StringBuilder SB = new StringBuilder();
            if (IsRegister)
            {
                SB.AppendFormat("<select name='{0}' class='{1}' id='{2}'  ng-model='registration.{0}' required  tabindex='4' style='font-size:16px;height: 3em;margin-top: 0;'><option value=''>Select City</option>", name, ClassName, id);
            }
            else
            {
                SB.AppendFormat("<select name='{0}' class='{1}' id='{2}' required ><option value=''>-- Select City --</option>", name, ClassName, id);
            }
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

        //Gender Dropdown List Helper
        public static IHtmlString GenderDropDownList(string name, string ClassName, string id, string selectdvalue, bool IsReg)
        {
            StringBuilder SB = new StringBuilder();
            if (IsReg)
            {
                SB.AppendFormat("<select name='{0}' class='{1}' id='{2}'  ng-model='profileDetail.{0}' required style='font-size:16px;height: 3em;margin-top: 0;'><option value=''>Gender</option>", name, ClassName, id);
            }
            else
            {
                SB.AppendFormat("<select name='{0}' class='{1}' id='{2}' ><option value=''>-- Select Gender --</option>", name, ClassName, id);
            }
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
                    SB.AppendFormat("<option value='Male'>Male</option>");
                    SB.AppendFormat("<option value='Female'>Female</option>");
                    SB.AppendFormat("<option value='All Male group' selected>All Male group</option>");
                    SB.AppendFormat("<option value='All Female Group' >All Female Group</option>");
                    SB.AppendFormat("<option value='Mixed Group' >Mixed Group</option>");
                }
                else if (selectdvalue == "All Male group")
                {
                    SB.AppendFormat("<option value='Male'>Male</option>");
                    SB.AppendFormat("<option value='Female'>Female</option>");
                    SB.AppendFormat("<option value='All Male group' >All Male group</option>");
                    SB.AppendFormat("<option value='All Female Group' selected>All Female Group</option>");
                    SB.AppendFormat("<option value='Mixed Group' >Mixed Group</option>");
                }
                else if (selectdvalue == "Mixed Group")
                {
                    SB.AppendFormat("<option value='Male' >Male</option>");
                    SB.AppendFormat("<option value='Female'>Female</option>");
                    SB.AppendFormat("<option value='All Male group'>All Male group</option>");
                    SB.AppendFormat("<option value='All Female Group' >All Female Group</option>");
                    SB.AppendFormat("<option value='Mixed Group' selected>Mixed Group</option>");
                }
                else
                {
                    SB.AppendFormat("<option value='Male' >Male</option>");
                    SB.AppendFormat("<option value='Female'>Female</option>");
                    SB.AppendFormat("<option value='All Male group'>All Male group</option>");
                    SB.AppendFormat("<option value='All Female Group' >All Female Group</option>");
                    SB.AppendFormat("<option value='Mixed Group'>Mixed Group</option>");
                }
            }

            //SB.AppendFormat("<option value='Deleted' >Deleted</option>");
            SB.Append("</select>");
            return new HtmlString(SB.ToString());
        }

        //Performing Members Type Dropdown List Helper
        public static IHtmlString PerformingMemDropDownList(string name, string ClassName, string id, string selectdvalue)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("<select name='{0}' class='{1}' id='{2}' ><option value=''>-- Select Performing Members --</option>", name, ClassName, id);
            if (String.IsNullOrEmpty(selectdvalue))
            {
                SB.AppendFormat("<option value='Individual' >Individual</option>");
                SB.AppendFormat("<option value='Duo' >Duo</option>");
                SB.AppendFormat("<option value='Group' >Group</option>");
            }
            else
            {
                if (selectdvalue == "Individual")
                {
                    SB.AppendFormat("<option value='Individual' selected>Individual</option>");
                    SB.AppendFormat("<option value='Duo' >Duo</option>");
                    SB.AppendFormat("<option value='Group' >Group</option>");
                }
                else if (selectdvalue == "Duo")
                {
                    SB.AppendFormat("<option value='Individual' >Individual</option>");
                    SB.AppendFormat("<option value='Duo' selected>Duo</option>");
                    SB.AppendFormat("<option value='Group' >Group</option>");
                }
                else if (selectdvalue == "Group")
                {
                    SB.AppendFormat("<option value='Individual' >Individual</option>");
                    SB.AppendFormat("<option value='Duo' >Duo</option>");
                    SB.AppendFormat("<option value='Group' selected>Group</option>");
                }
                else
                {
                    SB.AppendFormat("<option value='Individual' >Individual</option>");
                    SB.AppendFormat("<option value='Duo' >Duo</option>");
                    SB.AppendFormat("<option value='Group' >Group</option>");
                }
            }

            //SB.AppendFormat("<option value='Deleted' >Deleted</option>");
            SB.Append("</select>");
            return new HtmlString(SB.ToString());
        }

        //Number of year's Performing Dropdown List Helper
        public static IHtmlString NYrPerformingDropDownList(string name, string ClassName, string id, string selectdvalue, bool IsReg)
        {
            StringBuilder SB = new StringBuilder();
            if (IsReg)
            {
                SB.AppendFormat("<select name='{0}' class='{1}' id='{2}'  ng-model='profileDetail.{0}' required ><option value=''>Number of years you have been performing</option>", name, ClassName, id);
            }
            else
            {
                SB.AppendFormat("<select name='{0}' class='{1}' id='{2}' ><option value=''>-- Number of years you have been performing --</option>", name, ClassName, id);
            }
            if (String.IsNullOrEmpty(selectdvalue))
            {
                SB.AppendFormat("<option value='0-1' >0-1</option>");
                SB.AppendFormat("<option value='1-3' >1-3</option>");
                SB.AppendFormat("<option value='3-5' >3-5</option>");
                SB.AppendFormat("<option value='5-10' >5-10</option>");
                SB.AppendFormat("<option value='10+' >10+</option>");
            }
            else
            {
                if (selectdvalue == "0-1")
                {
                    SB.AppendFormat("<option value='0-1' Selected>0-1</option>");
                    SB.AppendFormat("<option value='1-3' >1-3</option>");
                    SB.AppendFormat("<option value='3-5' >3-5</option>");
                    SB.AppendFormat("<option value='5-10' >5-10</option>");
                    SB.AppendFormat("<option value='10+' >10+</option>");
                }
                else if (selectdvalue == "1-3")
                {
                    SB.AppendFormat("<option value='0-1' >0-1</option>");
                    SB.AppendFormat("<option value='1-3' Selected>1-3</option>");
                    SB.AppendFormat("<option value='3-5' >3-5</option>");
                    SB.AppendFormat("<option value='5-10' >5-10</option>");
                    SB.AppendFormat("<option value='10+' >10+</option>");
                }
                else if (selectdvalue == "3-5")
                {
                    SB.AppendFormat("<option value='0-1' >0-1</option>");
                    SB.AppendFormat("<option value='1-3' >1-3</option>");
                    SB.AppendFormat("<option value='3-5' Selected>3-5</option>");
                    SB.AppendFormat("<option value='5-10' >5-10</option>");
                    SB.AppendFormat("<option value='10+' >10+</option>");
                }
                else if (selectdvalue == "5-10")
                {
                    SB.AppendFormat("<option value='0-1' >0-1</option>");
                    SB.AppendFormat("<option value='1-3' >1-3</option>");
                    SB.AppendFormat("<option value='3-5' >3-5</option>");
                    SB.AppendFormat("<option value='5-10' Selected>5-10</option>");
                    SB.AppendFormat("<option value='10+' >10+</option>");
                }
                else if (selectdvalue == "10+")
                {
                    SB.AppendFormat("<option value='0-1' >0-1</option>");
                    SB.AppendFormat("<option value='1-3' >1-3</option>");
                    SB.AppendFormat("<option value='3-5' >3-5</option>");
                    SB.AppendFormat("<option value='5-10' >5-10</option>");
                    SB.AppendFormat("<option value='10+' Selected>10+</option>");
                }
                else
                {
                    SB.AppendFormat("<option value='0-1' >0-1</option>");
                    SB.AppendFormat("<option value='1-3' >1-3</option>");
                    SB.AppendFormat("<option value='3-5' >3-5</option>");
                    SB.AppendFormat("<option value='5-10' >5-10</option>");
                    SB.AppendFormat("<option value='10+' >10+</option>");
                }
            }

            //SB.AppendFormat("<option value='Deleted' >Deleted</option>");
            SB.Append("</select>");
            return new HtmlString(SB.ToString());
        }

        //Number of show Dropdown List Helper
        public static IHtmlString NoOfShowDropDownList(string name, string ClassName, string id, string selectdvalue, bool IsReg)
        {
            StringBuilder SB = new StringBuilder();

            if (IsReg)
            {
                SB.AppendFormat("<select name='{0}' class='{1}' id='{2}'  ng-model='profileDetail.{0}' required style='font-size:16px;height: 3em;margin-top: 0;' ><option value=''>Number of shows performed in</option>", name, ClassName, id);
            }
            else
            {
                SB.AppendFormat("<select name='{0}' class='{1}' id='{2}' ><option value=''>-- Select Number Of Shows --</option>", name, ClassName, id);
            }


            if (String.IsNullOrEmpty(selectdvalue))
            {
                SB.AppendFormat("<option value='50' >50</option>");
                SB.AppendFormat("<option value='100' >100</option>");
                SB.AppendFormat("<option value='200' >200</option>");
                SB.AppendFormat("<option value='500' >500</option>");
                SB.AppendFormat("<option value='1000' >1000</option>");
                SB.AppendFormat("<option value='2000' >2000</option>");
            }
            else
            {
                if (selectdvalue == "50")
                {
                    SB.AppendFormat("<option value='50' Selected>50</option>");
                    SB.AppendFormat("<option value='100' >100</option>");
                    SB.AppendFormat("<option value='200' >200</option>");
                    SB.AppendFormat("<option value='500' >500</option>");
                    SB.AppendFormat("<option value='1000' >1000</option>");
                    SB.AppendFormat("<option value='2000' >2000</option>");
                }
                else if (selectdvalue == "100")
                {
                    SB.AppendFormat("<option value='50' >50</option>");
                    SB.AppendFormat("<option value='100' Selected>100</option>");
                    SB.AppendFormat("<option value='200' >200</option>");
                    SB.AppendFormat("<option value='500' >500</option>");
                    SB.AppendFormat("<option value='1000' >1000</option>");
                    SB.AppendFormat("<option value='2000' >2000</option>");
                }
                else if (selectdvalue == "200")
                {
                    SB.AppendFormat("<option value='50' >50</option>");
                    SB.AppendFormat("<option value='100' >100</option>");
                    SB.AppendFormat("<option value='200' Selected>200</option>");
                    SB.AppendFormat("<option value='500' >500</option>");
                    SB.AppendFormat("<option value='1000' >1000</option>");
                    SB.AppendFormat("<option value='2000' >2000</option>");
                }
                else if (selectdvalue == "500")
                {
                    SB.AppendFormat("<option value='50' >50</option>");
                    SB.AppendFormat("<option value='100' >100</option>");
                    SB.AppendFormat("<option value='200' >200</option>");
                    SB.AppendFormat("<option value='500' Selected>500</option>");
                    SB.AppendFormat("<option value='1000' >1000</option>");
                    SB.AppendFormat("<option value='2000' >2000</option>");
                }
                else if (selectdvalue == "1000")
                {
                    SB.AppendFormat("<option value='50' >50</option>");
                    SB.AppendFormat("<option value='100' >100</option>");
                    SB.AppendFormat("<option value='200' >200</option>");
                    SB.AppendFormat("<option value='500' >500</option>");
                    SB.AppendFormat("<option value='1000' Selected>1000</option>");
                    SB.AppendFormat("<option value='2000' >2000</option>");
                }
                else if (selectdvalue == "2000")
                {
                    SB.AppendFormat("<option value='50' >50</option>");
                    SB.AppendFormat("<option value='100' >100</option>");
                    SB.AppendFormat("<option value='200' >200</option>");
                    SB.AppendFormat("<option value='500' >500</option>");
                    SB.AppendFormat("<option value='1000' >1000</option>");
                    SB.AppendFormat("<option value='2000' Selected>2000</option>");
                }
                else
                {
                    SB.AppendFormat("<option value='50' >50</option>");
                    SB.AppendFormat("<option value='100' >100</option>");
                    SB.AppendFormat("<option value='200' >200</option>");
                    SB.AppendFormat("<option value='500' >500</option>");
                    SB.AppendFormat("<option value='1000' >1000</option>");
                    SB.AppendFormat("<option value='2000' >2000</option>");
                }
            }

            //SB.AppendFormat("<option value='Deleted' >Deleted</option>");
            SB.Append("</select>");
            return new HtmlString(SB.ToString());
        }

        //Performance Length Dropdown List Helper
        public static IHtmlString PerfLengthDropDownList(string name, string ClassName, string id, string selectdvalue, bool IsReg)
        {
            StringBuilder SB = new StringBuilder();

            if (IsReg)
            {
                SB.AppendFormat("<select name='{0}' class='{1}' id='{2}'  ng-model='profileDetail.{0}' required ><option value=''>Select Performance Length</option>", name, ClassName, id);
            }
            else
            {
                SB.AppendFormat("<select name='{0}' class='{1}' id='{2}' ><option value=''>-- Select Performance Length --</option>", name, ClassName, id);
            }

            if (String.IsNullOrEmpty(selectdvalue))
            {
                SB.AppendFormat("<option value='30' >30</option>");
                SB.AppendFormat("<option value='60' >60</option>");
                SB.AppendFormat("<option value='120' >120</option>");
            }
            else
            {
                if (selectdvalue == "30")
                {
                    SB.AppendFormat("<option value='30' Selected>30</option>");
                    SB.AppendFormat("<option value='60' >60</option>");
                    SB.AppendFormat("<option value='120' >120</option>");
                }
                else if (selectdvalue == "60")
                {
                    SB.AppendFormat("<option value='30' >30</option>");
                    SB.AppendFormat("<option value='60' Selected>60</option>");
                    SB.AppendFormat("<option value='120' >120</option>");
                }
                else if (selectdvalue == "120")
                {
                    SB.AppendFormat("<option value='30' >30</option>");
                    SB.AppendFormat("<option value='60' >60</option>");
                    SB.AppendFormat("<option value='120' Selected>120</option>");
                }
                else
                {
                    SB.AppendFormat("<option value='30' >30</option>");
                    SB.AppendFormat("<option value='60' >60</option>");
                    SB.AppendFormat("<option value='120' >120</option>");
                }
            }

            //SB.AppendFormat("<option value='Deleted' >Deleted</option>");
            SB.Append("</select>");
            return new HtmlString(SB.ToString());
        }

        //Performance Language Dropdown List Helper
        public static IHtmlString PerformanceLanguageDropDownList(string name, string ClassName, string id, string selectdvalue, bool IsReg)
        {
            StringBuilder SB = new StringBuilder();
            if (IsReg)
            {
                SB.AppendFormat("<select name='{0}' class='{1}' id='{2}'  ng-model='profileDetail.{0}' required style='font-size:16px;height: 3em;margin-top: 0;'><option value=''>Select performance language</option>", name, ClassName, id);
            }
            else
            {
                SB.AppendFormat("<select name='{0}' class='{1}' id='{2}' ><option value=''>-- Select Performance Language --</option>", name, ClassName, id);
            }
            if (String.IsNullOrEmpty(selectdvalue))
            {
                SB.AppendFormat("<option value='English' >English</option>");
                SB.AppendFormat("<option value='Hindi' >Hindi</option>");
                SB.AppendFormat("<option value='Multilingual' >Multilingual</option>");
            }
            else
            {
                if (selectdvalue == "English")
                {
                    SB.AppendFormat("<option value='English' Selected>English</option>");
                    SB.AppendFormat("<option value='Hindi'>Hindi</option>");
                    SB.AppendFormat("<option value='Multilingual' >Multilingual</option>");
                }
                else if (selectdvalue == "Hindi")
                {
                    SB.AppendFormat("<option value='English' >English</option>");
                    SB.AppendFormat("<option value='Hindi' Selected>Hindi</option>");
                    SB.AppendFormat("<option value='Multilingual' >Multilingual</option>");
                }
                else if (selectdvalue == "Multilingual")
                {
                    SB.AppendFormat("<option value='English' >English</option>");
                    SB.AppendFormat("<option value='Hindi' >Hindi</option>");
                    SB.AppendFormat("<option value='Multilingual' Selected>Multilingual</option>");
                }
                else
                {
                    SB.AppendFormat("<option value='English' >English</option>");
                    SB.AppendFormat("<option value='Hindi' >Hindi</option>");
                    SB.AppendFormat("<option value='Multilingual' >Multilingual</option>");
                }
            }

            SB.Append("</select>");
            return new HtmlString(SB.ToString());
        }

        //Register as(Entertaine/Talent Seeker) Dropdown List Helper
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

        //Category Language Dropdown List Helper
        public static IHtmlString CategoryDropDownList(string name, string ClassName, string id, string selectdvalue, bool IsReg)
        {
            StringBuilder SB = new StringBuilder();

            if (IsReg)
            {
                SB.AppendFormat("<select name='{0}' class='{1}' id='{2}'  ng-model='category.{0}' required style='font-size:16px;height: 3em;margin-top: 0;'><option value=''>Select Category</option>", name, ClassName, id);
            }
            else
            {
                SB.AppendFormat("<select name='{0}' class='{1}' id='{2}' ><option value=''>-- Select Category --</option>", name, ClassName, id);
            }

            if (String.IsNullOrEmpty(selectdvalue))
            {
                var cats = db.tbl_Category.OrderBy(f => f.Title).ToList();
                foreach (var ct in cats)
                {
                    if (ct.IsActive == true)
                    {
                        SB.AppendFormat("<option value='{0}'>{1}</option>", ct.CatId, ct.Title);
                    }
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

        public static IHtmlString TLGenderDropDownList(string name, string ClassName, string id, string selectdvalue)
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("<select name='{0}' class='{1}' id='{2}' ><option value=''>-- Select Gender --</option>", name, ClassName, id);
            if (String.IsNullOrEmpty(selectdvalue))
            {
                SB.AppendFormat("<option value='Male' >Male</option>");
                SB.AppendFormat("<option value='Female' >Female</option>");
                SB.AppendFormat("<option value='Other' >Other</option>");
            }
            else
            {
                if (selectdvalue == "Male")
                {
                    SB.AppendFormat("<option value='Male' selected>Male</option>");
                    SB.AppendFormat("<option value='Female'>Female</option>");
                    SB.AppendFormat("<option value='Other' >Other</option>");
                }
                else if (selectdvalue == "Female")
                {
                    SB.AppendFormat("<option value='Male'>Male</option>");
                    SB.AppendFormat("<option value='Female' selected>Female</option>");
                    SB.AppendFormat("<option value='Other' >Other</option>");
                }
                else if (selectdvalue == "Other")
                {
                    SB.AppendFormat("<option value='Male' >Male</option>");
                    SB.AppendFormat("<option value='Female'>Female</option>");
                    SB.AppendFormat("<option value='Other' selected>Other</option>");
                }

            }

            //SB.AppendFormat("<option value='Deleted' >Deleted</option>");
            SB.Append("</select>");
            return new HtmlString(SB.ToString());
        }

        //Fetch State Name Helper Function
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

        //Fetch City Name Helper Function
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

        //Fetch Entertainer Name Helper Function
        public static string GetEntCatName(long? id)
        {
            if (id == null)
                return "-";
            var cat = db.tbl_EntertainerCats.Where(d => d.EntrId == id).FirstOrDefault();
            if (cat == null)
                return "-";
            else
            {
                var length = cat.tbl_Category.Title.Length - 1;
                return cat.tbl_Category.Title.Substring(0, length);
            }

        }

        //Convert and Use Youtube Image from the url
        public static string GetYouTubeVideoImage(string YoutubeUrl)
        {
            string youTubeThumb = string.Empty;
            if (YoutubeUrl == "")
                return "";

            if (YoutubeUrl.IndexOf("=") > 0)
                youTubeThumb = YoutubeUrl.Split('=')[1];
            else if (YoutubeUrl.IndexOf("/v/") > 0)
            {
                string strVideoCode = YoutubeUrl.Substring(YoutubeUrl.IndexOf("/v/") + 3);
                int ind = strVideoCode.IndexOf("?");
                youTubeThumb = strVideoCode.Substring(0, ind == -1 ? strVideoCode.Length : ind);
            }
            else if (YoutubeUrl.IndexOf('/') < 6 && !YoutubeUrl.Contains("embed"))
                youTubeThumb = YoutubeUrl.Split('/')[3];
            else if (YoutubeUrl.IndexOf('/') > 6)
                youTubeThumb = YoutubeUrl.Split('/')[1];
            else if (YoutubeUrl.Contains("youtu.be"))
            {
                var lstindx = YoutubeUrl.LastIndexOf('/') + 1;
                youTubeThumb = YoutubeUrl.Substring(lstindx, (YoutubeUrl.Length - lstindx));
            }
            else if (YoutubeUrl.Contains("embed"))
            {
                var lstindx = YoutubeUrl.LastIndexOf('/') + 1;
                youTubeThumb = YoutubeUrl.Substring(lstindx, (YoutubeUrl.Length - lstindx));
            }

            return "https://img.youtube.com/vi/" + youTubeThumb + "/mqdefault.jpg";
        }

        public static string ConvertYTLink(string url)
        {
            try
            {
                if (url.Contains("embed"))
                    return url;
                else if (url.Contains("watch"))
                {
                    var lstindx = url.LastIndexOf('=') + 1;
                    string convertedurl = "";
                    convertedurl = "https://www.youtube.com/embed/" + url.Substring(lstindx, (url.Length - lstindx)) + "?autoplay=1&rel=0&html5=0";
                    return convertedurl;
                }
                else if (url.Contains("youtu.be"))
                {
                    var lstindx = url.LastIndexOf('/');
                    string convertedurl = "";
                    convertedurl = "https://www.youtube.com/embed/" + url.Substring(lstindx, (url.Length - lstindx)) + "?autoplay=1&rel=0&html5=0";
                    return convertedurl;
                }
                return url;
            }
            catch (Exception)
            {
                return url;
            }

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
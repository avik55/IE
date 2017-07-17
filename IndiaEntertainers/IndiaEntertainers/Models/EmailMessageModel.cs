using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndiaEntertainers.Models
{
    public class EmailMessageModel
    {
        // Summary:
        //     Message contents
        public virtual string Body { get; set; }
        //
        // Summary:
        //     Destination, i.e. To email, or SMS phone number
        public virtual string Destination { get; set; }
        //
        // Summary:
        //     Subject
        public virtual string Subject { get; set; }
    }
}
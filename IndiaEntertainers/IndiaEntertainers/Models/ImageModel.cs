using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IndiaEntertainers.Models
{
    public class ImageModel
    {
        public long? Id { get; set; }

        [StringLength(250)]
        public string Title { get; set; }

        [StringLength(350)]
        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public HttpPostedFileBase Photo { get; set; }

    }
}
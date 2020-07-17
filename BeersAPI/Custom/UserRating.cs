using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeersAPI.Custom
{
    public class UserRating
    {
        public int id { get; set; }
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "username should be a valid email address. eg: john@vintri-tech.ca")]
        public string username { get; set; }
        public int rating { get; set; }
        public string comments  { get; set; }
    }
}
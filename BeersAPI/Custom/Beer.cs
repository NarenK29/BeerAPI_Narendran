using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeersAPI.Custom
{
    public class Beer
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description  { get; set; }        
        public List<UserRating> userRatings { get; set; }
    }
}
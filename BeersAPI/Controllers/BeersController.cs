using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using BeersAPI.Custom;
using Newtonsoft.Json;




namespace BeersAPI.Controllers
{
    public class BeersController : ApiController
    {
        private IPathProvider jsonPath;
        public BeersController(IPathProvider pathProvider)
        {
            this.jsonPath = pathProvider;
        }
        public BeersController()
        {
            this.jsonPath = new ServerPathProvider();
        }
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("api/Beers/AddRating")]
        [UserNameActionFilter]
        [HttpPost]
        //Get Load Information        
        public string AddRating(string id,[FromBody] UserRating userRating)
        {
            string message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(id) && userRating != null && !string.IsNullOrEmpty(userRating.username) && userRating.rating > 0)
                {
                    int beerID = 0;
                    if (int.TryParse(id, out beerID))
                    {
                        if (beerID != 0)
                        {
                            //Validate Beer Id
                            if (ValidateBeer(beerID))
                            {
                                //Validate user rating (range 1-5)
                                if(userRating.rating >= 1 && userRating.rating <= 5)
                                {
                                    userRating.id = beerID;
                                    //Add to database.json
                                    var dbjson = jsonPath.MapPath();
                                    // Read existing json data
                                    var jsonData = System.IO.File.ReadAllText(dbjson);
                                    // De-serialize to object or create new list
                                    var beersUserRating = JsonConvert.DeserializeObject<List<UserRating>>(jsonData)
                                                          ?? new List<UserRating>();

                                    // Add new user Ratings
                                    beersUserRating.Add(userRating);
                                     // Update json data string
                                     jsonData = JsonConvert.SerializeObject(beersUserRating);
                                     File.WriteAllText(dbjson, jsonData);

                                    //Send Success Message to User
                                    message = "Thank you for your rating. Your response is added successfully!";
                                }
                                else
                                {
                                    message = "Rating should be in the range 1 to 5";
                                }

                            }
                            else
                            {
                                return "Beer doesnot exist! Please check Beer Id";
                            }
                        }
                        else
                        {
                            message = "Invalid Beer Id";
                        }
                    }
                    else
                    {
                        message = "ID should be an integer";
                    }
                }
                else
                {
                    message = "Input parameters are invalid. Hint: username should be an email id and ratings should be an integer between 1 to 5";
                }
            }
            catch (Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Beers_API";
                    eventLog.WriteEntry("Error while adding Rating to Beer : " + ex.ToString(), EventLogEntryType.Error, 101, 1);
                    message = ex.Message;
                }
            }
            return message;
        }



        [Route("api/Beers/GetBeers")]
        [HttpGet]
        //Get Load Information        
        public List<Beer> GetBeers(string name)
        {
            List<Beer> beers = new List<Beer>();
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                   
                    var dbjson = jsonPath.MapPath();
                    // Read existing json data
                    var jsonData = System.IO.File.ReadAllText(dbjson);
                    // De-serialize to object or create new list
                    var beersUserRating = JsonConvert.DeserializeObject<List<UserRating>>(jsonData)
                                          ?? new List<UserRating>();

                    var searchUrl = "https://api.punkapi.com/v2/beers?beer_name=" + name;
                    HttpWebRequest request = WebRequest.CreateHttp(searchUrl);
                    request.Method = "GET";
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8))
                            {
                                string responseJSON = myStreamReader.ReadToEnd();

                                var beersData = JsonConvert.DeserializeObject<dynamic>(responseJSON);                                                                
                                if (beersData != null)
                                {
                                    int beerId = 0;
                                    foreach (var data in beersData)
                                    {
                                        Beer beerObj = new Beer();
                                        beerObj.id = data.id;
                                        beerObj.name = data.name;
                                        beerObj.description = data.description;
                                        //Linq to get userratings for matching BeerId and add to List of User Ratings
                                        if (!string.IsNullOrEmpty(Convert.ToString(data.id)))
                                        {
                                            beerId = Convert.ToInt32(data.id);
                                            var userRatings = beersUserRating.Where(s => s.id == beerId).ToList();
                                            if(userRatings.Count > 0)
                                            {
                                                beerObj.userRatings = userRatings;
                                            }
                                        }
                                        beers.Add(beerObj);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Beers_API";
                    eventLog.WriteEntry("Error while getting Beers by name: " + ex.ToString(), EventLogEntryType.Error, 101, 1);                   
                }
            }
            return beers;
        }

        public static bool ValidateBeer(int id)
        {
            bool isValid = false;
            try
            {
                if (id > 0)
                {
                    var url = "https://api.punkapi.com/v2/beers/" + id;

                    HttpWebRequest request = WebRequest.CreateHttp(url);
                    request.Method = "GET";
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {                           
                            using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8))
                            {                                
                                string responseJSON = myStreamReader.ReadToEnd();

                                var data = JsonConvert.DeserializeObject<dynamic>(responseJSON);
                                string tempId = Convert.ToString(data[0].id);
                                tempId = tempId.Trim();
                                string tempname = Convert.ToString(data[0].name);
                                if (!string.IsNullOrEmpty(tempId) && !string.IsNullOrEmpty(tempname))
                                {
                                    int validatedId = 0;
                                    if (int.TryParse(tempId, out validatedId))
                                    {
                                        if (validatedId == id)
                                        {
                                            isValid = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "BeersAPI";
                    eventLog.WriteEntry("Error While Validating Beer Id " + ex.ToString(), EventLogEntryType.Error, 101, 1);
                }
            }
            return isValid;
        }


    }


    public interface IPathProvider
    {
        string MapPath();
    }

    public class ServerPathProvider : IPathProvider
    {
        public string MapPath()
        {
            return HttpContext.Current.Server.MapPath(@"~/UserRatings/database.json");
        }
    }

    public class TestPathProvider : IPathProvider
    {
        public string MapPath()
        {
            return Path.Combine(@"C:\Demo\database.json");
        }
    }

   
}

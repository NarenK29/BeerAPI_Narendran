using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeersAPI;
using BeersAPI.Controllers;
using BeersAPI.Custom;
using System.Collections.Generic;
using System.Linq;

namespace BeersAPI.Tests.Controllers
{
    [TestClass]
    public class BeersControllerTest
    {
        [TestMethod]
        public void TestGetBeersEndPointForErrors()
        {
            // Arrange
            BeersController controller = new BeersController(new TestPathProvider());
            string beerName = "Trashy Blonde";

            // Act
            var actualBeerNamefromEndPoint = string.Empty;
            var actualBeers = controller.GetBeers(beerName) as List<BeersAPI.Custom.Beer>;
            if(actualBeers.Count > 0)
            {
                actualBeerNamefromEndPoint = actualBeers.First().name;
            }

            // Assert            
            Assert.IsNotNull(actualBeerNamefromEndPoint);
            Assert.AreEqual(beerName, actualBeerNamefromEndPoint);
            
        }

        [TestMethod]
        public void TestForAddRatingEndPoint()
        {
            // Arrange
            BeersController controller = new BeersController(new TestPathProvider());
            string beerId = "2";
            UserRating ratingObj = new UserRating();
            ratingObj.rating = 4;
            ratingObj.username = "test@abc.ca";
            ratingObj.comments = "Nice";

            string actualResponse = "Thank you for your rating. Your response is added successfully!";
            // Act            
            var response = controller.AddRating(beerId, ratingObj);
           

            // Assert            
            Assert.IsNotNull(response);
            Assert.AreEqual(actualResponse, response);

        }

        [TestMethod]
        public void TestForRatingRange()
        {
            // Arrange
            BeersController controller = new BeersController(new TestPathProvider());
            string beerId = "2";
            UserRating ratingObj = new UserRating();
            ratingObj.rating = 9;
            ratingObj.username = "test@abc.ca";
            ratingObj.comments = "Very Nice";

            string actualResponse = "Rating should be in the range 1 to 5";
            // Act            
            var response = controller.AddRating(beerId, ratingObj);


            // Assert            
            Assert.IsNotNull(response);
            Assert.AreEqual(actualResponse, response);

        }

        
    }
}

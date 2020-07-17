API EndPoints

1. Add Rating
EndPoint: api/Beers/AddRating
Method: POST
Input:
Parameter: id (int/string)
body: 
username(string),rating(integer),comments(string)
eg:
{
    "username":"naren@dc.ca",
    "rating":5,
    "comments":"Loved it"
}

Output: 
Message
eg: 
"Rating should be in the range 1 to 5"
"Thank you for your rating. Your response is added successfully!"

2. GetBeers
EndPoint: api/Beers/GetBeers
Method: GET
Input:
Parameter: name(string)
eg: 
body: Trashy Blonde

Output(sample below): Beers Object Collection in below format (includes User Ratings)
[
    {
        "id": 2,
        "name": "Trashy Blonde",
        "description": "A titillating, neurotic, peroxide punk of a Pale Ale. Combining attitude, style, substance, and a little bit of low self esteem for good measure; what would your mother say? The seductive lure of the sassy passion fruit hop proves too much to resist. All that is even before we get onto the fact that there are no additives, preservatives, pasteurization or strings attached. All wrapped up with the customary BrewDog bite and imaginative twist.",
        "userRatings": [
            {
                "id": 2,
                "username": "narena@dc.ca",
                "rating": 5,
                "comments": "Loved it"
            }
        ]
    },
    {
        "id": 270,
        "name": "Blonde Export Stout",
        "description": "Our Equity Punk gypsy brewery - Beatnik Brewing Collective - voted for this recipe as their 2017 annual brew. A blonde stout brewed to export strength, the stout character comes from extra ingredients instead of dark malts.",
        "userRatings": null
    },
    {
        "id": 277,
        "name": "Prototype Blonde Ale",
        "description": "This Blonde Ale is a new recipe and uses a new yeast - so it is a true Prototype. Originally lined up as part of the 2017 Prototype Challenge it was released shortly afterwards. but hey, these things happen.",
        "userRatings": null
    }
]



3. Added ActionFilters to validate username
	used RegEx in class (Data Annotation)


4.Unit Test (3 Methods)
a.TestGetBeersEndPointForErrors
b.TestForAddRatingEndPoint
c.TestForRatingRange
Note: Please create a file database.json under C:\Demo. (Added dependency injection to handle Server Path of database.json)



## Create the database

Migration assembly is already created. To create the database, 
- First configure the connection string in `appsettings.json` (or leave it as it is).
- In Visual Studio, go to **Tools > NuGet Package Manager > Package Manager Console**
- Write `Update-Database` and press enter.

## Populate the data

Copy the following JSON and decorate with your data, post it to the following url from any API Client (ex. Postman, Insomnia, Nightingale REST Client)

```json
[
    {
        "title": "ASP.NET Core in Action, Third Edition",
        "author" : "Andrew Lock",
        "description" : "",
        "price" : 0,
        "imageUrl" : "https://learning.oreilly.com/library/cover/9781633438620/250w/",
        "isbn": "9781633438620",
        "publishedOn" : "2023-08-01T18:25:43.511Z",
        "publisher" : "Manning Publications",
        "seoText" : ""
    }
]
```
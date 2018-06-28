# KodisoftTestAssignment

### Nota Bene:
For those, who wants to work with this project:
- MS SQL DB is not docked in repo. You need to create it on your own computer via following:
  - Change in application.json DB's connectionString to whatever you want
  - In Visual Studio Package Manager Console tab:
    ```
    Add-Migration Initial
    Update-Database
    ```
- Logging is established via Docker-ElasticSearch-Kibana stack. To work with logging you need:
  - install Docker
  - open KodisoftTestAssignment/KodisoftTestAssignment/docker folder in command line
  - run the docker compose command to spin up the containers:
    ```
    docker-compose up -d
    ```
  -  first time you run this command it can take some time to load images from docker registry
  -  once it's completed, check that ElasticSearch and Kibana are up and running (I am personally using Kitematic for that)


### Used technologies:
- ASP .Net Core 2.0;
- SQL (mssqllocaldb) and EF Core 2.0: db interactions;
- MemoryCache: for feed caching;
- RestSharp: for api testing and presentation;
- Docker-ElasticSearch-Kibana-Serilog: for logging;
- oAuth 2.0: authentication;

## Description
### Back-end developer task
Create an ASP .NET WebAPI (4 or Core) back-end web service for providing news streams from different sources. Service should function like Feedly service (https://feedly.com/).Service should allow clients to manage feed collections, and to access all news items in those collections. Service should extensively cache data to minimize external requests. As a start, use RSS or Atom feeds, but architecture should be extensible to support adding new types of feed sources in the future (3rdparty services, web scraping, etc). Service should return data in its own format, you should create custom models for all data types, not just redirect XML feeds to the clients. This should be a headless Web API REST service, no UI is necessary. 

Necessary functions:
- Create a new collection (returns collection Id)
- Add feed to a collection
- Get all news for a collection
- Caching of feed data
- A simple (console or GUI) test application to show API interaction

Plus will be:
- C#/.net client SDK
- Authentication
- Persistence
- Logging
- Tests

Projects will be tested on latest Visual Studio 2017 Community/Pro with Core and Web tools installed. 

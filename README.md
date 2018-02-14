# T2D
ThingToData

Source code

Use this version (master2_0 is still in development).

Usage guide: ThingToData CURL ThingStories.docx
Swagger: http://thingtodata.azurewebsites.net/swagger.json

How to create a new Inventory
* Install SQL Server 2016 database instance for T2D (Azure Sql Database is also OK)
* Update connection strings in appsettings.json -files (in each project)
* Run T2D.Infra project, it will delete and create T2D database. It can also add seed data to it.
* Run T2D.InventoryAPI project. It can be deployed to Azure Web App.

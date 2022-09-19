# todos
Simple TODOs app
The project should have just three main features:

- A list of textual to-do items that can be checked off
- A way to add new items - it can either be a button or a more refined mechanism of your choice
- A search label to perform fuzzy search on the items

# Development environment
- Visual Studio Code 
- netcore 6.0.302
- node 16.16.0
- Angular CLI: 13.3.4

The backend connects to an existing Azure Cognitive Search account and index.
To set local development secrets, create *appsettings.Development.json* files and override the needed values.
These files are ignored and won't be pushed to GitHub.

# Run locally
Backend runs on https://localhost:5050. Spa client runs on https://localhost:44418.
When running the backend from VS Code, the SPA proxy will start the front end automatically and redirect to it when it is ready.
To browse Swagger API documentation for the backend, use https://localhost:44418/swagger.

# Publish
To publish from VS Code, install Azure Account and Azure App Service extensions and perform *Azure: SignIn*

Go to *Todos* folder and run 
`dotnet publish -c Release`

Go to *Todos/bin/Release/net6.0* folder, right click on *publish*, select *Deploy to WebApp* and pick the App Service to publish to.

# Note
To have the backend and SPA proxy run from Visual Studio, some changes are needed in the project or launch files.
Current setup was only tested with VS Code.



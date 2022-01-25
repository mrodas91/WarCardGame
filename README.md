# WarCardGame
 War Game Client
 
## Prerequisites
On client machine:
- Docker
- NetCore SDK 6 or ASP.NET Core Runtime 6 and .NET Runtime 6.0.1  (https://dotnet.microsoft.com/en-us/download/dotnet/6.0)


## Way to use
### Clone Repositories
1. Open your terminal and navigate to the folder you want to work
2. Clone both repositories by running below commands:
   - WarCardGame: `git clone git@github.com:mrodas91/WarCardGame.git`
   - WardGameAPI: `git clone git@github.com:mrodas91/WarGameAPI.git`

### Create a MS SQL Server Container in Docker
A docker container is needed in order to storage the service db.
In the terminal run this command: 
- `docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=Passw0rd!' -p 1433:1433 --name dockersqledge -d mcr.microsoft.com/azure-sql-edge`

### Start WarGameAPI Service
Using your terminal
1. Navigate to WarGameAPI folder path (Inside the folder is a .csproj file).
2. Execute `docker run` to start the service. It, by default, will run in localhost using por 5001 for https and 5000 for http
3. Once the service starts, open your browser and go to https://localhost:5001/swagger/index.html to open the API documentation in Swagger.
4. Search for AddPlayer method in the Players secction and Expand.
5. Click on `Try it out` button and **ADD 2 New Player** passing below info in the Request body section :
  - For Player 1 `{
  "nombre": "Matt"
}` then click `Execute` button.
  - For Player 2 `{
  "nombre": "Andrew"
}` then click `Execute` button.

You can create as many player you want (2 Player are required).

### Run War Game
1. Open a new terminal (Previos one is now running the service and wont be possible to use).
2. Navigate to WarCardGame folder path (Inside the folder is a .csproj file).
3. Execute `dotnet run` and the game will start

*Notes: You also can run the game using `dotnet run v` when `v` is the argunt to tell the program to display the initail decks for each player and both decks during each round during the game.*

### Check Results
1. Open your browser again and go to the API documentation in Swagger (https://localhost:5001/swagger/index.html).
2. Search for GetAllPlayers method in the **Players** secction and Expand.
3. Click on `Try it out` and then click on `Execute`. You will see the statistics for each player.
4. Now Search for `GetAllMatches` method in the **Matches** secction and Expand.
5. Click on `Try it out` and then click on `Execute`. It will display all matches results.

*Notes: There are other edpoints you can use inside the documentation that works with the playerId and matchId*


# SupportDesk

## Project Overview
SupportDesk is an ASP.NET Core Web API project targeting .NET 10. It provides support-oriented tooling and chat endpoints backed by SQL Server and Entity Framework Core. The application includes order and shipment tool services, customer-context middleware, and OpenAPI/Scalar integration for API exploration in development.

Alongside the source code, I’m also posting video on my Youtube Channel, the playlist link is https://youtube.com/playlist?list=PLZImzUtvBwnk&si=R381-A0LsJFzbqde where I build this project step-by-step from scratch.

## Setup and Run Steps
1. **Install prerequisites**
   - .NET 10 SDK
   - SQL Server (local or container)
   - Microsoft Foundry Account

2. **Configure the database connection and Foundry account**
   - Update `SupportDesk/appsettings.json` if needed.
   - Ensure `ConnectionStrings:DefaultConnection` points to a reachable SQL Server instance. 
   - Configure your Foundry account credentials in the appropriate settings file.

3. **Restore dependencies**
   - From the repository root, run:
	 - `dotnet restore SupportDesk.slnx`\
     
4. **Database Changes**
   - From the repository root, run:
	 - `dotnet ef database update --project SupportDesk`
     -  

4. **Run the application**
   - Start the API:
	 - `dotnet run --project SupportDesk/SupportDesk.csproj`
     -  Run the SQL Script in order to insert Sample Data 

5. **Access API docs in development**
   - Open the OpenAPI/Scalar endpoint exposed by the app when running in Development.

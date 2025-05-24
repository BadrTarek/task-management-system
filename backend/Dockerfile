FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["Src/Presentation/Presentation.csproj", "Src/Presentation/"]
COPY ["Src/Application/Application.csproj", "Src/Application/"]
COPY ["Src/Domain/Domain.csproj", "Src/Domain/"]
COPY ["Src/Data/Data.csproj", "Src/Data/"]
RUN dotnet restore "Src/Presentation/Presentation.csproj"

# Copy the rest of the code
COPY . .

# Build and publish
RUN dotnet build "Src/Presentation/Presentation.csproj" -c Release -o /app/build
RUN dotnet publish "Src/Presentation/Presentation.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "Presentation.dll"] 
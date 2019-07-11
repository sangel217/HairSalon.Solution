FROM mcr.microsoft.com/dotnet/core/sdk:2.2 

WORKDIR ./HairSalon

COPY . .

RUN ls

WORKDIR ./HairSalon

RUN dotnet restore 

RUN dotnet publish ./HairSalon.csproj -o /publish/

WORKDIR /publish

ENTRYPOINT ["dotnet", "HairSalon.dll"]
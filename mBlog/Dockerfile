FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

#Copy csproj and restore dependencies
COPY mBlog.csproj .
RUN dotnet restore 

#Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o /app/publish

#Use the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app
COPY --from=build /app/publish .

#Expose default port
EXPOSE 80
#EXPOSE 443

#ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password=2016June01#


#Set the entry point
ENTRYPOINT [ "dotnet", "mBlog.dll" ]
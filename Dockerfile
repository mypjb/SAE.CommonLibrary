FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder

WORKDIR /code

COPY . .

RUN ["dotnet","nuget","add","source","https://nuget.cdn.azure.cn/v3/index.json","-n","azure.org"]

RUN dotnet build -c Release

RUN ["dotnet","pack","-c","Release","--no-build","--include-source","--output","nupkgs"]

RUN ["dotnet","nuget","push","nupkgs/*.symbols.nupkg","-k","111111","-s","http://nuget.lass.net"]


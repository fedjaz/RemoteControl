FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

RUN mkdir -p /home/src/server

ADD Server /home/src/server/Server
ADD SharedProjects/Encryption /home/src/server/SharedProjects/Encryption

RUN dotnet publish /home/src/server/Server -o /home/src/server/build


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS RUN

RUN mkdir -p /home/src/server

COPY --from=build /home/src/server/build /home/src/server

CMD ["dotnet", "/home/src/server/Server.dll"]
docker run -p 1433:1433 -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=EventSourceT3st!" --name mssql microsoft/mssql-server-linux:2017-latest

REM docker exec -it 6f97c1cc9448 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "EventSourceT3st!" -Q "CREATE DATABASE MyDb0"

docker cp src/InexRef.EventSourcing.Tests.SqlServer.Setup.Db/bin/debug/InexRef.EventSourcing.Tests.SqlServer.Setup.Db.dacpac mssql:InexRef.EventSourcing.Tests.SqlServer.Setup.Db.dacpac

docker exec -it mssql wget https://dot.net/v1/dotnet-install.sh  

docker exec -it mssql chmod +x dotnet-install.sh

docker exec -it mssql ./dotnet-install.sh -c Current

docker exec -it mssql apt-get update -qq 

docker exec -it mssql apt-get install -y curl apt-transport-https software-properties-common locales unzip

docker exec -it mssql curl -Lq https://go.microsoft.com/fwlink/?linkid=873926 -o sqlpackage-linux-x64-latest.zip

docker exec -it mssql unzip sqlpackage-linux-x64-latest.zip -d opt/sqlpackage

docker exec -it mssql root/.dotnet/dotnet opt/sqlpackage/sqlpackage.dll /tsn:localhost /tu:SA /tp:EventSourceT3st! /A:Import /td n:InexRef.EventSourcing.Tests /sf:InexRef.EventSourcing.Tests.SqlServer.Setup.Db.dacpac

docker exec -it mssql root/.dotnet/dotnet opt/sqlpackage/sqlpackage.dll /tsn:localhost /tu:SA /tp:"EventSourceT3st!" /Action:Publish /tdn:InexRef.EventSourcing.Tests /sf:InexRef.EventSourcing.Tests.SqlServer.Setup.Db.dacpac

root/.dotnet/dotnet opt/sqlpackage/sqlpackage.dll /tsn:localhost /tu:SA /tp:"EventSourceT3st!" /Action:Import /td n:InexRef.EventSourcing.Tests /sf:InexRef.EventSourcing.Tests.SqlServer.Setup.Db.dacpac
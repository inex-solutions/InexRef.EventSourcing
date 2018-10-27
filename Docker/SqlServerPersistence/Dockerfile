FROM microsoft/mssql-server-linux:2017-latest

WORKDIR .

ENV SA_PASSWORD EventSourceT3st!
ENV ACCEPT_EULA Y

ADD InexRef.EventSourcing.Tests.SqlServer.Setup.Db\bin\Debug\InexRef.EventSourcing.Tests.SqlServer.Setup.Db.dacpac InexRef.EventSourcing.Tests.SqlServer.Setup.Db.dacpac
#RUN apt-get update -qq 
#RUN apt-get install -y curl apt-transport-https software-properties-common locales unzip

#RUN curl -Lq https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
#RUN chmod +x dotnet-install.sh
#RUN ./dotnet-install.sh -c Current

#RUN curl -Lq https://go.microsoft.com/fwlink/?linkid=873926 -o sqlpackage-linux-x64-latest.zip
#RUN unzip sqlpackage-linux-x64-latest.zip -d opt/sqlpackage
#RUN 
#RUN root/.dotnet/dotnet opt/sqlpackage/sqlpackage.dll /tsn:localhost /tu:SA /tp:"EventSourceT3st!" /Action:Publish /tdn:InexRef.EventSourcing.Tests /sf:InexRef.EventSourcing.Tests.SqlServer.Setup.Db.dacpac
#EXPOSE 1433
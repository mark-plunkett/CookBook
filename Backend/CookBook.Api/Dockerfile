FROM mcr.microsoft.com/dotnet/aspnet:5.0

ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.9.0/wait /wait
RUN chmod +x /wait

COPY bin/Release/net5.0/publish/ App/
WORKDIR /App
ENV DOTNET_EnableDiagnostics=0
CMD /wait && dotnet CookBook.Api.dll

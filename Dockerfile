FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as publish
WORKDIR /src
COPY . ./

RUN dotnet restore

RUN dotnet publish YASDM.Client -c Release -o /publish-client
RUN dotnet publish YASDM.Api -c Release -r alpine-x64 --self-contained true /p:PublishTrimmed=true /p:PublishSingleFile=true -o /publish-server

FROM nginx:alpine as final

RUN apk add --no-cache libstdc++ libintl bind-tools 

EXPOSE 80

# Copy 
WORKDIR /usr/share/nginx/html
RUN rm -f /usr/share/nginx/html/index.html
COPY --from=publish /publish-client/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf


WORKDIR /api
COPY --from=publish /publish-server .
COPY start.sh .

ENV WAIT_VERSION 2.7.2
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/$WAIT_VERSION/wait /wait
RUN chmod +x /wait

CMD ["./start.sh"]
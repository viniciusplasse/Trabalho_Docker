# Trabalho_Docker
https://hub.docker.com/_/microsoft-dotnet-core-aspnet/

$ docker build -t imagem-docker-trabalho -f Dockerfile .

docker create --name trabalho-core-container imagem-docker-trabalho

$ docker container start trabalho-core-container


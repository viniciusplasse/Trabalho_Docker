# Trabalho_Docker
Docker Hub: https://hub.docker.com/_/microsoft-dotnet-core-aspnet/

$ docker build -t imagem-docker -f Dockerfile .

$ docker create --name trabalho-container imagem-docker

$  docker container start trabalho-container

$ docker container run -p 5005:80 imagem-docker



FROM microsoft/dotnet:runtime
MAINTAINER valery.sntx@gmail.com
LABEL Name=expressionutil Version=3.0.0 
ARG source=.
WORKDIR /expressionutil
COPY $source .
ENTRYPOINT sh
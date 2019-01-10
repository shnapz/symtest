# dotnet

FROM microsoft/dotnet:2.2-sdk AS build

WORKDIR /

COPY *.sln ./

COPY symtest/symtest.csproj symtest/
COPY symtest.Tests/symtest.Tests.csproj symtest.Tests/
COPY symtest.Tests.Integration/symtest.Tests.Integration.csproj symtest.Tests.Integration/
COPY symtest.Common/symtest.Common.csproj symtest.Common/
COPY symtest.Logic/symtest.Logic.csproj symtest.Logic/
COPY symtest.Client/symtest.Client.csproj symtest.Client/

RUN dotnet restore
COPY . .
WORKDIR /symtest
RUN dotnet build -c Release -o /app -r alpine.3.6-x64

FROM build AS publish
RUN dotnet publish -c  Release -o /app/linux -r linux-x64
RUN dotnet publish -c Release -o /app/alpine -r alpine.3.6-x64

FROM microsoft/dotnet:2.2-runtime-deps-alpine AS base
WORKDIR /app
COPY --from=publish /app/linux/libuv.so .
COPY --from=publish /app/alpine .

# RabbitMQ

FROM rabbitmq:3.7.8-management-alpine
CMD ["rabbitmq-server"]

# Actual launch

ENTRYPOINT [ "/app/symtest" ]
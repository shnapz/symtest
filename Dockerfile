FROM microsoft/dotnet:2.2-sdk-alpine3.8 AS build
WORKDIR /app

COPY *.sln ./

COPY symtest/symtest.csproj symtest/
COPY symtest.Tests/symtest.Tests.csproj symtest.Tests/
COPY symtest.Tests.Integration/symtest.Tests.Integration.csproj symtest.Tests.Integration/
COPY symtest.Common/symtest.Common.csproj symtest.Common/
# COPY symtest.Logic/symtest.Logic.csproj symtest.Logic/
COPY symtest.Client/symtest.Client.csproj symtest.Client/

WORKDIR /app/symtest
RUN dotnet restore

WORKDIR /app/
COPY . .
WORKDIR /app/symtest
RUN dotnet publish -c Release -o out

# test application -- see: dotnet-docker-unit-testing.md
#FROM build AS testrunner
#WORKDIR /app/tests
#COPY tests/. .
#ENTRYPOINT ["dotnet", "test", "--logger:trx"]

FROM rabbitmq:3.7.8-management-alpine
CMD ["rabbitmq-server"]

FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine3.8 AS runtime
WORKDIR /app
COPY --from=build /app/symtest/out ./
ENTRYPOINT ["dotnet", "symtest.dll"]
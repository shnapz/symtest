FROM microsoft/dotnet:2.2-sdk-alpine3.8 AS build
WORKDIR /app

COPY *.sln ./

COPY symtest/symtest.csproj symtest/
COPY symtest.Tests/symtest.Tests.csproj symtest.Tests/
COPY symtest.Tests.Integration/symtest.Tests.Integration.csproj symtest.Tests.Integration/
COPY symtest.Common/symtest.Common.csproj symtest.Common/
COPY symtest.Client/symtest.Client.csproj symtest.Client/

WORKDIR /app/symtest
RUN dotnet restore

WORKDIR /app/
COPY . .
WORKDIR /app/symtest
RUN dotnet publish -c Debug -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine3.8 AS runtime
WORKDIR /app
COPY --from=build /app/symtest/out ./
ENTRYPOINT ["dotnet", "symtest.dll"]
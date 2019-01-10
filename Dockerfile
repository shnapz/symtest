FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY *.sln ./
COPY symtest/symtest.csproj symtest/
RUN dotnet restore
COPY . .
WORKDIR /src/symtest
RUN dotnet build -c Release -o /app -r alpine.3.6-x64

FROM build AS publish
RUN dotnet publish -c  Release -o /app/linux -r linux-x64
RUN dotnet publish -c Release -o /app/alpine -r alpine.3.6-x64

FROM microsoft/dotnet:2.2-runtime-deps-alpine AS base
WORKDIR /app
COPY --from=publish /app/linux/libuv.so .
COPY --from=publish /app/alpine .
ENTRYPOINT [ "/app/symtest" ]
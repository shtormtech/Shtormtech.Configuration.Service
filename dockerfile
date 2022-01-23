FROM mcr.microsoft.com/dotnet/sdk:5.0-focal as build-env
WORKDIR /app

# switch to root
USER 0
RUN mkdir obj

# Copy csproj and restore as distinct layers
COPY . ./

RUN dotnet restore shtormteh.configuration.service.sln

# run unit tets - build will fail if tests fail
#RUN dotnet test Tests

# build
RUN dotnet publish shtormtech.configuration.service/shtormtech.configuration.service.csproj -c Release -o /app/out --no-restore

RUN ls /app/out

# Build runtime image
FROM  mcr.microsoft.com/dotnet/aspnet:5.0-focal AS runtime
EXPOSE 80
WORKDIR /app
COPY --from=build-env /app/out .
# RUN mkdir /app/lib/ && cp /app/runtimes/win-x64/native/git2-106a5f2.dll /app/lib/git2-106a5f2.dll
ENTRYPOINT ["dotnet", "shtormtech.configuration.service.dll"]
# Configuration service
The service is designed to issue configuration files from a Git repository on a REST request

## How To Build and Run From Source
### .NET App
```bash
    $dotnet run --project ./shtormtech.configuration.service\shtormtech.configuration.service.csproj
```
Service available on [localhost:5000](http://localhost:5000)
### Docker
```bash
    $docker build -t config.service .
    $docker run \
        -e BaseConfiguration__Git__Uri="{GIT_REPO}" \
        -e BaseConfiguration__Git__Branch="master" \
        -e BaseConfiguration__Git__User="{GIT_USER}" \
        -e BaseConfiguration__Git__password="{GIT_PASSWORD}" \
        -p 8080:80 \
        config.service
```
Service available on [localhost:8080](http://localhost:8080)
## Usage example
examle repo:
```bash
 repo
    ├───service1
    │    └───service.appsettings.json
    ├───service2
    │   ├───front-end-config.json
    │   ├───deployment.yaml
    │   └───backend-appsetings.json
    └───readme.md
```
``` bash
    $curl http://localhost:8080/files/service1/service.appsettings.json
    {
        "config-key": "value"
    }
```
``` bash
    $curl http://localhost:8080/files/service2/backend-appsetings.json?branch=dev
    {
        "config-key": "value-in-dev-branch"
    }
```

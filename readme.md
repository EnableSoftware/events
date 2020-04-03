# Events

'Events' is a long lived hackathon project with the purpose of exploring bleeding edge technologies to discover what does and doesn't work well going forward in the world of .NET. It intends to become a fully functional generic booking system in the long term.

## Current technologies / features

- Blazor WASM
- Postgres
- ASPNET core
- Service workers / PWA
- Azure AD
- Gravatar

## Getting Started

### Dependencies

- [Latest .NET core SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop)

#### Until a better SCSS compilation solution is found

- Visual Studio 2019
- Visual Studio 2019 extension [Web Compiler](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.WebCompiler)


### Local development

#### User secrets

This project currently uses AzureAD exclusively, as such an instance will be required to authenticate.

The following settings are required:
- Domain
- ClientId
- TenantId

Execute the following to configure this: 
`./scripts/set-user-secrets.cmd`

#### Running the server

At the root of the project run `start.cmd`.

This will:
- Start docker instances of dependencies required
- Run DB migrations and seed data.
- Start the application at https://localhost:5001 (http://localhost:5000)

## License

Distributed under the MIT License. See `LICENSE` for more information.
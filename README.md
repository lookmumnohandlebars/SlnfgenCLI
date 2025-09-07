<div align="center">

![Slnfgen](https://raw.githubusercontent.com/lookmumnohandlebars/SlnfgenCLI/refs/heads/main/docs/assets/slnfgen-logo.png)

[![NuGet](https://img.shields.io/nuget/v/Slnfgen.CLI.svg)](https://www.nuget.org/packages/Slnfgen.CLI) [![NuGet Downloads](https://img.shields.io/nuget/dt/slnfgen.cli)](https://www.nuget.org/packages/Slnfgen.CLI)
[![Build](https://github.com/lookmumnohandlebars/slnfgencli/actions/workflows/main.merge.yml/badge.svg)](https://github.com/lookmumnohandlebars/slnfgencli/actions/workflows/main.merge.yml)
[![GitHub Stars](https://img.shields.io/github/stars/lookmumnohandlebars/slnfgencli.svg)](https://github.com/lookmumnohandlebars/slnfgencli/stargazers) [![GitHub license](https://img.shields.io/github/license/lookmumnohandlebars/slnfgencli)](https://img.shields.io/github/license/lookmumnohandlebars/slnfgencli) ![Dependabot](https://img.shields.io/badge/dependabot-enabled-025E8C?logo=dependabot&logoColor=white) ![CodeQL](https://github.com/lookmumnohandlebars/slnfgencli/actions/workflows/github-code-scanning/codeql/badge.svg)

### 🎉 A tool for managing monorepos with automated solution filter generation 🎉

---

</div>

### ❓ What Is It?

A CLI, or dotnet tool, that extends the .NET solution tooling with automated solution filter generation from a manifest file.

Made with love by @lookmumnohandlebars

### ❓ Why Would You Use It?

If you've ever had a monolith, or distributed system, with multiple applications and services, you'll know the pain of keeping a single solution organised and loadable by an IDE! Solution Filters were developed by the Visual Studio team to support this, however they become difficult to maintain.

- **Mono-repo tooling**: Allows automated maintenance of a .NET monorepo - similar to NX!
- **Cross-Platform**: Works on Windows, Linux & Mac!
- **Built with MSBuild**: Uses the same engine that Visual Studio and Rider uses to load solutions!

## 📦 Installation

### Local (recommended)

To install local to a repository, [setup a tool-manifest file](https://learn.microsoft.com/en-us/dotnet/core/tools/local-tools-how-to-use#create-a-manifest-file). To automatically add Slnfgen.CLI to the manifest

```bash
dotnet tool install Slnfgen.CLI
```

This can then be run as:

```
dotnet tool run slnfgen
```

### Global

To install globally:

```bash
dotnet tool install Slnfgen.CLI -g
```

This can then be run as:

```bash
dotnet slfgen
```

## ▶️ Usage

### Manifest File

The Manifest File is used to define all solution filters for a given solution. Both JSON and YAML file formats are supported (the schema can be found [here](./schema/manifest-file.schema.json))

Here is an example Manifest File:

```yml
solutionFile: Contoso.slnx # sln & slnx supported
autoIncludeSuffixPatterns:
  - Tests
filterDefinitions:
  - name: ContosoProducts # becomes FilterOne.slnf
    entrypoints:
      - Contoso.Products/Contoso.Products.csproj
      - Contoso.Products.Tests/Contoso.Products.Tests.csproj
    autoIncludeSuffixPatterns: Benchmarks

  - name: ContosoPayments
    entrypoints:
      - Payments/Contoso.Payments/Payments.csproj
      - tests/Contoso.Payments.Tests/Contoso.Payments.Tests.csproj
```

- `solutionFile`: The path to the "Parent" solution file
- `filterDefinitions`: Each filter is defined by a `name` (the name of the Solution Filter), and `entryPoints` which are projects at the top of the dependency tree (usually these will be the deployed projects and test projects)
- `autoIncludeSuffixPatterns`: These are the suffix patterns to automatically include in the solution filter. Primarily

### Generating Filters

With a valid Manifest file, the CLI can then be executed to generate the declared solution filters

```bash
# Generate all solution filters defined in the manifest.yml file
slfngen all manifest.yml

# Generate only the MyFilter solution filter in the manifest.yml file
slnfgen target manifest.yml -t MyFilter
```

## 📑 More Docs

All useful documentation can be found in this readme - I promise! For the eager amongst you out there, there are some other useful bits for finding your way around using and improving this repo (viewable on GitHub):

- **`--help`**: Appending this to any command will give you the usual.
- [**Reference**](./docs/reference.md): A complete documented reference of the CLI commands
- [**Changelog**](./docs/CHANGELOG.md): An automated list of the changes
- [**License**](./LICENSE): The open-source license ofs this repo
- [**Contribution Guide**](./docs/CONTRIBUTING.md): Contribution guidelines for those wanting to get involved and improve this tool!
- [**Code Of Conduct**](./docs/CODE_OF_CONDUCT.md): A defined way of behaving with others contributing.

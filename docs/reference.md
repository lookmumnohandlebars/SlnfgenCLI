# Reference

The `--help` option can be used to describe the commands in the command line

## Manifest File

The manifest file, required for defining solution filters, is a yaml or json file, which defines

- `.sln` & `.slnx` formats are supported
- `.sln`

```yml
solutionFile: Contoso.sln # Path to target solution file (from this file)
autoIncludeSuffixPatterns:
  - Tests

filterDefinitions:
  - name: Products # Solution Filter Name
    entrypoints:
      - Products.WebApi/Products.WebApi.csproj
      - tests/Products.UnitTests/Products.UnitTests.csproj
    autoIncludeSuffixPatterns:
      - Contracts
      - Benchmarks

  - name: Orders
    entrypoints:
      - ProjB/Nested/ProjB.csproj
      - ProjD/ProjD.csproj
```

## Commands

### `all`

Generates solution filters from a given manifest file, writing each filter to a `.slnf` file

```bash
slnfgen all [<MANIFEST_FILE>] [-o|--output <OUTPUT_DIRECTORY>]
    [--dry-run]
```

#### Example Usage

```bash
slnfgen all manifest.yml -o ./src
```

#### Options

| Option           | Description                                                          |
| ---------------- | -------------------------------------------------------------------- |
| `-o`, `--output` | The output directory for the solution filters. Defaults to `.`       |
| `--dry-run`      | Runs the generation command without saving the solution filter files |

### `target`

Generates a target solution filter from a given manifest file, writing the filter to a `.slnf` file

```bash
slnfgen target [<MANIFEST_FILE>] [-t|--target <TARGET_FILTER>]
    [-o|--output <OUTPUT_DIRECTORY>]
    [--dry-run]
```

#### Example Usage

```bash
slnfgen target manifest.yml -t MyFilter -o ./src
```

#### Manifest File

The manifest file, given in the argument, is a yaml or json file, which defines

- `.sln` & `.slnx` formats are supported
- `.sln`

```yml
solutionFile: Contoso.sln # Path to target solution file (from this file)
autoIncludeSuffixPatterns:
  - Tests
filterDefinitions:
  - name: Products # Solution Filter Name
    entrypoints:
      - Products.WebApi/Products.WebApi.csproj
      - tests/Products.UnitTests/Products.UnitTests.csproj
    autoIncludeSuffixPatterns:
      - Benchmarks

  - name: Orders
    entrypoints:
      - ProjB/Nested/ProjB.csproj
      - ProjD/ProjD.csproj
```

#### Options

| Option           | Description                                                          |
| ---------------- | -------------------------------------------------------------------- |
| `-o`, `--output` | The output directory for the solution filters. Defaults to `.`       |
| `--dry-run`      | Runs the generation command without saving the solution filter files |

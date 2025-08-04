# Reference

The `--help` option can be used to describe the commands in the command line

## Commands

### `gen`

Generates solution filters from a given manifest file, writing each filter to a `.slnf` file

```bash
slnfgen [<MANIFEST_FILE>] [-o|--output <OUTPUT_DIRECTORY>]
    [--dry-run]
```

#### Example Usage

```bash
slnfgen gen manifest.yml -o ./src
```

#### Manifest File

The manifest file, given in the argument, is a yaml or json file, which defines

- `.sln` & `.slnx` formats are supported
-

```yml
solutionFile: Contoso.sln # Path to target solution file (from this file)
filterDefinitions:
  - name: Products # Solution Filter Name
    entrypoints:
      - Products.WebApi/Products.WebApi.csproj
      - tests/Products.UnitTests/Products.UnitTests.csproj

  - name: Orders
    entrypoints:
      - ProjB/Nested/ProjB.csproj
      - ProjD/ProjD.csproj
```

#### Options

| Option           | Descrption                                                           |
| ---------------- | -------------------------------------------------------------------- |
| `-o`, `--output` | The output directory for the solution filters. Defaults to `.`       |
| `--dry-run`      | Runs the generation command without saving the solution filter files |

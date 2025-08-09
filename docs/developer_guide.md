# Developer Guide

## Dependencies

At any given point the dependencies can be checked against the `setup-dependencies` github action [here](../.github/actions/setup-dependencies/action.yml), however these should remain consistently used:

- .NET 9
- Taskfile
- Bun

You can run `task compatible` to check if your system is compatible

## Code Architecture

This repo uses a lightweight form of clean architecture (lightweight as it is not in separate projects - at least until the codebase warrants that!)

- **Domain**: Core models and functionality
- **Application**: Thing that handles the requests
- **Presentation**: The CLI level responsible for printing and showing things to the user

The tests are as follows

- **Unit**: Standard unit tests that ensure coverage and intentional functionality of classes
- **Integration**: Run the program from the command line, assert the main use cases.

## Working With The Repo

- **Taskfile**: All commonly used commands are encapsulated in taskfiles for ease of re-use. Run `task` in the command line at the root of the repo for a full list of commands.
- **Husky**: Husky is a tool built on top of git hooks. I would like a clean commit history where possible so husky is used to ensure that locally you get feedback on whether or not your code is broken. It also handles auto-formatting so there should be no inconsistencies between different developers.

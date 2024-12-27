# SourceGit Stripdown

This repository is a fork of https://github.com/sourcegit-scm/sourcegit  

The objective of SourceGit Stripdown is to progressively remove functionality from the upstream until only the following remains:

1. A commit graph.
1. A diff view.

## Basic commands
Run
```
cd src
dotnet run
```

Publish application to a folder (for Linux)
```
rm -rf ~/sourcegit-stripdown
dotnet publish -c Release -r linux-x64 -o ~/sourcegit-stripdown src/SourceGit.csproj
```

#!/bin/bash
set -o errexit

base_dir=$(cd $(dirname $0) && pwd)

app_key=${1:-"111111"}

nuget_source=${2:-"https://nuget.mirror.lass.net/v3/index.json"}

release_dir=${3:-"nupkgs"}

dotnet build -c release

dotnet test -v q -l "console;verbosity=detailed"

rm -rf $release_dir

dotnet pack --no-build --include-source -c release --output $release_dir

cd $release_dir

dotnet nuget push '*.symbols.nupkg' -k ${app_key} -s ${nuget_source}

echo "build end"
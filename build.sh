#!/bin/bash

base_dir=$(cd $(dirname $0) && pwd)

app_key=$1

nuget_source=$2

release_dir=$3

dotnet build -c release

dotnet test -v n


dotnet pack --no-build --include-source -c release --output $release_dir

cd $release_dir

dotnet nuget push '*.symbols.nupkg' -k ${app_key} -s ${nuget_source}

echo "build end"
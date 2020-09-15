#!/bin/bash

base_dir=$(cd $(dirname $0) && pwd)

app_key=$1

nuget_source=$2

release_dir=$3

dotnet build -c Release

dotnet test -v n


dotnet pack -c Release --no-build --include-source --output $release_dir

cd $release_dir

dotnet nuget push '*.symbols.nupkg' -k ${app_key} -s ${nuget_source}

done
#!/bin/bash
set -o errexit

base_dir=$(cd $(dirname $0) && pwd)

release_dir=${3:-"${HOME}/.nuget/locals/"}

dotnet build -c release

#本地构建不进行单元测试
#dotnet test -v q -l "console;verbosity=detailed"

dotnet pack --no-build -c release --output $release_dir

echo "build end"

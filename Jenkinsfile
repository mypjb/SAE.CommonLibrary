pipeline {
  agent {
    docker {
      image 'mypjb/dotnet-core-sdk:3.1'
      args '''-v /home/mypjb/.nuget/packages:/root/.nuget/packages
-v /etc/resolv.conf:/etc/resolv.conf'''
    }

  }
  stages {
    stage('Build') {
      steps {
        sh '''dotnet nuget add source ${DOTNET_NUGET_SOURCE} -n azure.org

dotnet build -c Release'''
      }
    }

    stage('Test') {
      steps {
        sh 'dotnet test -v n'
      }
    }

    stage('Create Nuget Package') {
      steps {
        sh '''rm -rf ${DOTNET_NUGET_DIR}
dotnet pack -c Release --no-build --include-source --output ${DOTNET_NUGET_DIR}'''
      }
    }

    stage('Publish Nuget Server') {
      steps {
        sh '''cd ${DOTNET_NUGET_DIR}
dotnet nuget push \'*.symbols.nupkg\' -k 111111 -s ${DOTNET_NUGET_PUBLISH_SOURCE}'''
      }
    }

  }
}
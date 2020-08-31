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
        sh '''${DOTNET_NUGET}
${DOTNET_BUILD}
'''
      }
    }

    stage('Test') {
      steps {
        sh 'dotnet test -t'
      }
    }

    stage('Create Nuget Package') {
      steps {
        sh '''rm -rf nupkgs
${DOTNET_NUGET_BUILD}'''
      }
    }

    stage('Publish Nuget Server') {
      steps {
        sh '$DOTNET_NUGET_PUBLISH'
      }
    }

  }
}
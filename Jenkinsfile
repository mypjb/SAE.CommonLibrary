pipeline {
  agent {
    docker {
      image 'mypjb/dotnet-core-sdk:5.0'
      args '-v nuget:/root/.nuget -v release:/root/release'
    }

  }
  stages {
    stage('Build') {
      steps {
        sh 'bash ./build.sh $NUGET_APPKEY $NUGET_SOURCE $RELEASE_DIR/Nuget'
      }
    }
  }
}
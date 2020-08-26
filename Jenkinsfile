pipeline {
  agent any
  stages {
    stage('build') {
      steps {
        sh '${BUILD_NUGET}'
      }
    }

  }
  environment {
    sdk = ''
  }
}
pipeline {
    agent {
        docker {
            image 'cpp:latest' // Используем образ Docker с необходимыми инструментами для сборки C++ проекта
        }
    }
    stages {
        stage('Checkout') {
            steps {
                git 'https://github.com/dinkledong/graph_search.git' // Клонируем ваш репозиторий
            }
        }
        stage('Build') {
            steps {
                sh 'make' // Используем make для сборки проекта
            }
        }
        stage('Archive') {
            steps {
                archiveArtifacts artifacts: '**/your_executable_file', fingerprint: true // Архивируем исполняемый файл
            }
        }
    }
    post {
        success {
            echo 'Build successful! Archiving artifacts...'
        }
    }
}
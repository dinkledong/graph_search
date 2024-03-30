pipeline {
    agent any // Используем любой доступный агент для запуска пайплайна

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/dinkledong/graph_search.git' // Клонируем ваш репозиторий, указывая ветку main
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

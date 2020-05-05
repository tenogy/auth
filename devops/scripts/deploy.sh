#!/bin/bash
source $projectDir/devops/scripts/vars.sh;

function deploy {
   
    readState
    echo "deploy the app ${appVersion}";


    sshC="-i $config_deploy_pk  $config_deploy_host";
    echo "ssh command: ${sshC}";

    echo "remove previous container versions"
    ssh ${sshC} "docker rmi \$(docker images ${config_imageName} -q) -f";

    echo save new image on remote server
    docker save $config_imageName:$appVersion | ssh -C $sshC docker load;

    echo stop app
    ssh ${sshC} "cd ${config_deploy_app} && docker-compose down -v --remove-orphans"
    echo copy docker-compose.yml
    scp -i $config_deploy_pk $projectDir/devops/docker-compose.yml $config_deploy_host:$config_deploy_app/docker-compose.yml
    echo fill environtment file
    ssh ${sshC} "cd ${config_deploy_app} && echo \"APP_VERSION=${appVersion}\" > .env && echo \"ASPNET_VERSION=${config_deploy_runtime}\" >> .env && echo \"APP_PORT=${config_deploy_port}\" >> .env"
    echo run app
    ssh ${sshC} "cd ${config_deploy_app} && docker-compose up -d"

    echo docker prune
    docker system prune -f
}
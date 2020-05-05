#!/bin/bash
source $projectDir/devops/scripts/vars.sh;

function build {
   echo "remove previos versions of images"
   docker rmi $(docker images -q $imageName) -f

    echo "increment build number"
    awk -F ': ' '/buildNumber/{$2=$2+1""}1' OFS=': ' $stateFile > _current.tmp && mv -f _current.tmp $stateFile

    readState

    echo "build the app ${appVersion}";
    cd $projectDir;
    echo docker build --no-cache -t $config_imageName:$appVersion --build-arg VERSION=$config_build_sdk -f $projectDir/devops/Dockerfile .
    docker build --no-cache -t $config_imageName:$appVersion --build-arg VERSION=$config_build_sdk -f $projectDir/devops/Dockerfile .
  
    
    exit
}

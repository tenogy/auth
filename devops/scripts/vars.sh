#!/bin/bash
scriptsDir=$projectDir/devops/scripts;
stateFile=${rootDir}/_current;
appVersion="";

source $scriptsDir/parse_yaml.sh;
#parse_yaml ${rootDir}/config.yml "config_"
eval $(parse_yaml ${rootDir}/config.yml "config_")

function readState {
    echo "read state"
    eval $(parse_yaml ${stateFile} "state_")
    appVersion="${config_version}.${state_buildNumber}";
}
#!/bin/bash
if [ "$TRAVIS_BRANCH" == "master" ]; then
    dotnet tool install coveralls.net --version 1.0.0 --tool-path tools        
    "./tools/csmacnz.Coveralls --opencover -i YASDM.Model.Tests/TestResults/coverage.opencover.xml --repoTokenVariable COVERALL_REPO_TOKEN"
fi
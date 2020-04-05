#!/bin/bash
if [ "$TRAVIS_BRANCH" == "master" ]; then
    dotnet tool install coveralls.net --version 1.0.0 --tool-path tools        
    "./tools/csmacnz.Coveralls --opencover -i YASDM.Model.Tests/TestResults/coverage.opencover.xml --repoTokenVariable COVERALL_REPO_TOKEN --commitId $TRAVIS_COMMIT --commitBranch $TRAVIS_BRANCH --commitAuthor "$REPO_COMMIT_AUTHOR" --commitEmail "$REPO_COMMIT_AUTHOR_EMAIL" --commitMessage "$REPO_COMMIT_MESSAGE" --jobId $TRAVIS_JOB_ID  --serviceName \"travis-ci\"" 
fi
#!/bin/sh

docker-compose down
cd storage/
sudo chmod ugo+rwx mongodb/* && sudo chmod ugo+rwx mongodb/diagnostic.data/* && sudo chmod ugo+rwx mongodb/journal/*
cd ..
docker-compose up --build
#!/bin/bash
sudo apt-get update
sudo apt-get -y install postgresql
psql postgresql://<user>:<password>@<host>/<db> << EOF
       <your sql queries go here>
EOF
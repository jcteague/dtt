#!/bin/sh
# This initial setup works for ubuntu
sudo apt-get -y install postgresql &&
sudo apt-get -y install nodejs npm &&
sudo apt-get -y install ruby rubygems &&
sudo apt-get -y install sendmail &&
sudo ./install_dependencies &&
sudo gem install pg -v '0.13.2' &&
sudo gem install bundle &&
sudo npm install -g coffee-script &&
sudo npm install -g mocha &&
bundle install &&
./setup_redis.sh

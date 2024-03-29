#!/bin/sh
# This initial setup works for ubuntu
sudo apt-get -y install postgresql &&
sudo apt-get -y install nodejs npm &&
sudo apt-get -y install ruby rubygems &&
sudo apt-get -y install sendmail &&
sudo apt-get -y install nginx &&
sudo ./install_dependencies &&
sudo gem install pg -v '0.13.2' &&
sudo gem install bundler &&
sudo npm install -g coffee-script &&
sudo npm install -g mocha &&
bundle install &&
./setup_redis.sh

echo '------------------------------------------------------------';
echo 'Finished installing dependencies';
echo '------------------------------------------------------------';

echo '------------------------------------------------------------';
echo 'Starting services';
echo '------------------------------------------------------------';
sudo service nginx start;

echo '------------------------------------------------------------';
echo 'Registering nginx in startup';
echo '------------------------------------------------------------';
update-rc.d nginx defaults;

echo '------------------------------------------------------------';
echo 'Creating log directories';
echo '------------------------------------------------------------';
mkdir development_logs;

echo '------------------------------------------------------------';
echo 'Creating the environment databases';
echo '------------------------------------------------------------';
sudo su postgres -c "createdb dtt_main" &&
sudo su postgres -c "createdb dtt_test";
echo '------------------------------------------------------------';
echo 'NOTE: Remember to set up your postgres user with the correct password'

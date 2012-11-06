#/bin/sh
if [[ -z "$1" ]]; then
    echo 'You must specify an environment: setup_local, setup_staging or setup_production';
    exit;
fi

echo '------------------------------------------------------------';
echo 'Installing stunnel, varnish and nginx';
echo '------------------------------------------------------------';
sudo apt-get install -y stunnel varnish nginx;

echo '------------------------------------------------------------';
echo 'Setting up the services using: '$1;
echo '------------------------------------------------------------';
sudo rake -f build_tools/Rakefile.rb $1;

echo '------------------------------------------------------------';
echo 'Restarting services with configuration';
echo '------------------------------------------------------------';
sudo service stunnel4 restart;
sudo service varnish restart;
sudo service nginx restart;

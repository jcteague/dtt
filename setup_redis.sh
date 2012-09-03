wget http://redis.googlecode.com/files/redis-2.4.15.tar.gz &&
tar xzf redis-2.4.15.tar.gz &&
cd redis-2.4.15 &&
make &&
rm -f ../redis-2.4.15.tar.gz &&
cp ../build_tools_templates/redis.conf . &&
cp ../build_tools_templates/redis-test.conf .

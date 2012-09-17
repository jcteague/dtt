db_config =
    user: 'postgres'
    password: '1234'
    host: 'localhost'
    db_main: 'dtt_main'
    db_test: 'dtt_test'

whitelisted_paths = ['/client', '/registration','/user/login','/']

development_settings =
    db:
        connection_string: "postgres://#{db_config.user}:#{db_config.password}@#{db_config.host}/#{db_config.db_main}"
    site:
        host: 'localhost'
        port: 3000
        client_ID: '1234'
        client_secret: 'secret'
        url: 'http://localhost:3000'
        whitelisted_paths: whitelisted_paths
    redis:
        host:'dtt.local'
        port: 6379
        password: 'welc0me'

test_settings =
    db:
        connection_string: "postgres://#{db_config.user}:#{db_config.password}@#{db_config.host}/#{db_config.db_test}"
    site:
        host: 'localhost'
        port: 3000
        client_ID: '1234'
        client_secret: 'secret'
        url: 'http://localhost:3000'
        whitelisted_paths: whitelisted_paths
    redis:
        host:'dtt.local'
        port: 6380
        password: 'welc0me'

production_settings =
    db:
        connection_string: "postgres://huyuuxyveqegxe:tMU5vspNvcoPxePlBbK5DX1Jvx@ec2-23-21-91-108.compute-1.amazonaws.com:5432/d9er2dp9rejk7k"
    site:
        host: 'ec2-107-21-171-44.compute-1.amazonaws.com'
        port: 80
        client_ID: '1234'
        client_secret: 'secret'
        url: 'http://ec2-107-21-171-44.compute-1.amazonaws.com'
        whitelisted_paths: whitelisted_paths
    redis:
        host: 'ec2-107-21-171-44.compute-1.amazonaws.com'
        port: 6379
        password: '15439fde8d415f7ab4a3cc9b389badea'

module.exports = ->
    switch process.env.NODE_ENV
        when 'test' then test_settings
        when 'production' then production_settings
        else development_settings

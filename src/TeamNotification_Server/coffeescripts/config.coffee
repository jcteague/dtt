db_config =
    user: 'postgres'
    password: '1234'
    host: 'localhost'
    db_main: 'dtt_main'
    db_test: 'dtt_test'

whitelisted_paths = ['/client', '/registration','/user/login']

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
        port: 6379

production_settings =
    db:
        connection_string: "postgres://huyuuxyveqegxe:tMU5vspNvcoPxePlBbK5DX1Jvx@ec2-23-21-91-108.compute-1.amazonaws.com:5432/d9er2dp9rejk7k"
    site:
        host: 'dtt.jit.su'
        port: 80
        client_ID: '1234'
        client_secret: 'secret'
        url: 'http://dtt.jit.su'
        whitelisted_paths: whitelisted_paths
    redis:
        host: ''
        port: 6379

module.exports = ->
    switch process.env.NODE_ENV
        when 'test' then test_settings
        when 'production' then production_settings
        else development_settings

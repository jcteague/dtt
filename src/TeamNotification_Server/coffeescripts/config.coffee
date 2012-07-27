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

production_settings =
    db:
        connection_string: "postgres://postgres:welc0me@localhost:5432/dtt_main"
    site:
        host: 'ec2-107-21-171-44.compute-1.amazonaws.com'
        port: 80
        client_ID: '1234'
        client_secret: 'secret'
        url: 'http://ec2-107-21-171-44.compute-1.amazonaws.com'
        whitelisted_paths: whitelisted_paths

module.exports = ->
    switch process.env.NODE_ENV
        when 'test' then test_settings
        when 'production' then production_settings
        else development_settings

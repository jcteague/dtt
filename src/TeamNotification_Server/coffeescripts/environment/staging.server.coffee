path = require('path')

db_config =
    user: 'postgres'
    password: '1234'
    host: 'localhost'
    db_main: 'dtt_main'
    db_test: 'dtt_test'

whitelisted_paths = ['/', '/client', '/api/', '/socket', /\/api\/user\/confirm\/*/, /\/api\/reset_password\/*/, /\/api\/forgot_password\/*/, '/api/registration','/api/user/login', '/api/plugin', /\/github\/events\/*/, /\/bitbucket\/events\/*/, /^\/api\/room\/.+\/accept-invitation$/]

data_path = path.join(__dirname, 'data')
visual_studio_plugin_json = path.join(data_path, 'vs.extension.json')
visual_studio_plugin_installer = path.join(data_path, 'TeamNotification_Package.vsix')

default_settings =
    env: 'staging'
    db:
        user: 'postgres'
        connection_string: "postgres://#{db_config.user}:#{db_config.password}@#{db_config.host}/#{db_config.db_main}"
    site:
        host: 'stagingdtt.local'
        port: 3000
        client_ID: '1234'
        client_secret: 'secret'
        url: 'https://stagingdtt.local'
        surl: 'https://stagingdtt.local'
        whitelisted_paths: whitelisted_paths
    api:
        host: 'api.dtt.local'
        port: 3000
        client_ID: '1234'
        client_secret: 'secret'
        url: 'https://stagingdtt.local:3000/api'
        whitelisted_paths: whitelisted_paths
    redis:
        host:'api.stagingdtt.local'
        port: 6379
        password: 'welc0me'
    email:
        user: 'AKIAJH4LGM6D7JTFT4MQ'
        password: 'AlFd3CC8EzPTn+GE301M9oQthfZU5aPPBmKemJtz4e4X'
    github:#FOR TEST PURPOSES, TODO: ask John for the real deal
        client_id: '238dc978aaf2621d38b5'
        secret: 'f86f03ae61ed557e0bb97cfbc25c5d0e43f0a350'
        redirect_url: 'https://stagingdtt.local:3001/api/github/auth/callback'
        state: 'zY6KPiIcKuhTYOdoUSX8avKc2mGASfNfHkvP50nAkPo='
    bitbucket:
        secret: "Wu4Hst6HFL8FzX5MGFzmKErtXKTg5628"
        key: "54GnwCv7MHJeBVWAU3"
        
    plugins:
        visual_studio:
            manifest: visual_studio_plugin_json
            installer: visual_studio_plugin_installer
    log:
        path: path.join('/var', 'log', 'yackety.log')
        token: ''
        logkey: 'Invalid during development ):'
        apikey: 'Invalid during development ):' #'ab92172f734744139af7e4edaed1ae1a'


test_settings =
    env: 'test'
    db:
        user: 'postgres'
        connection_string: "postgres://#{db_config.user}:#{db_config.password}@#{db_config.host}/#{db_config.db_test}"
    site:
        host: 'stagingdtt.local'
        port: 3000
        client_ID: '1234'
        client_secret: 'secret'
        url: 'https://stagingdtt.local'
        surl: 'https://stagingdtt.local'
        whitelisted_paths: whitelisted_paths
    api:
        host: 'api.stagingdtt.local'
        port: 3001
        client_ID: '1234'
        client_secret: 'secret'
        url: 'https://staginglocal/api'
        whitelisted_paths: whitelisted_paths
    redis:
        host:'api.stagingdtt.local'
        port: 6380
        password: 'welc0me'
    email:
        user: 'AKIAJH4LGM6D7JTFT4MQ'
        password: 'AlFd3CC8EzPTn+GE301M9oQthfZU5aPPBmKemJtz4e4X'
    github:#FOR TEST PURPOSES, TODO: ask John for the real deal
        client_id: '238dc978aaf2621d38b5'
        secret: 'f86f03ae61ed557e0bb97cfbc25c5d0e43f0a350'
        redirect_url: 'https://stagingdtt.local:3001/api/github/auth/callback'
        state: 'zY6KPiIcKuhTYOdoUSX8avKc2mGASfNfHkvP50nAkPo='
    bitbucket:
        secret: "Wu4Hst6HFL8FzX5MGFzmKErtXKTg5628"
        key: "54GnwCv7MHJeBVWAU3"
        
    plugins:
        visual_studio:
            manifest: visual_studio_plugin_json
            installer: visual_studio_plugin_installer
    log:
        path: path.join(process.cwd(), '..', '..', 'development_logs', 'test.log')
        token: ''
        logkey: 'Invalid during testing ):' #'Yackety'
        apikey: 'Invalid during testing ):'#'ab92172f734744139af7e4edaed1ae1a'
        

module.exports = ->
    switch process.env.NODE_ENV
        when 'test' then test_settings
        else default_settings

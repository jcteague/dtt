path = require('path')

db_config =
    user: 'postgres'
    password: '1234'
    host: 'localhost'
    db_main: 'dtt_main'
    db_test: 'dtt_test'

whitelisted_paths = ['/', '/client', '/registration','/user/login', '/plugin', /\/github\/events\/*/, /^\/room\/.+\/accept-invitation$/]

data_path = path.join(__dirname, 'data')
visual_studio_plugin_json = path.join(data_path, 'vs.extension.json')
visual_studio_plugin_installer = path.join(data_path, 'TeamNotification_Package.vsix')

development_settings =
    env: 'development'
    db:
        connection_string: "postgres://#{db_config.user}:#{db_config.password}@#{db_config.host}/#{db_config.db_main}"
    site:
        host: 'dtt.local'
        port: 3000
        client_ID: '1234'
        client_secret: 'secret'
        url: 'http://dtt.local:3000'
        surl: 'https://dtt.local:3001'
        whitelisted_paths: whitelisted_paths
    api:
        host: 'api.dtt.local'
        port: 3001
        client_ID: '1234'
        client_secret: 'secret'
        url: 'https://api.dtt.local:3001'
        whitelisted_paths: whitelisted_paths
    redis:
        host:'api.dtt.local'
        port: 6379
        password: 'welc0me'
    email:
        user: 'AKIAJH4LGM6D7JTFT4MQ'
        password: 'AlFd3CC8EzPTn+GE301M9oQthfZU5aPPBmKemJtz4e4X'
    github:#FOR TEST PURPOSES, TODO: ask John for the real deal
        client_id: '238dc978aaf2621d38b5'
        secret: 'f86f03ae61ed557e0bb97cfbc25c5d0e43f0a350'
        redirect_url: 'http://api.dtt.local:3000/github/auth/callback'
        state: 'zY6KPiIcKuhTYOdoUSX8avKc2mGASfNfHkvP50nAkPo='
    plugins:
        visual_studio:
            manifest: visual_studio_plugin_json
            installer: visual_studio_plugin_installer
    log:
        path: path.join(process.cwd(), 'development_logs', 'dev.log')
        token: ''
        logkey: 'eespinal'
        apikey: '38af1eb61bad44a0b8ca6db044d672ff'


test_settings =
    env: 'test'
    db:
        connection_string: "postgres://#{db_config.user}:#{db_config.password}@#{db_config.host}/#{db_config.db_test}"
    site:
        host: 'dtt.local'
        port: 3000
        client_ID: '1234'
        client_secret: 'secret'
        url: 'http://dtt.local:3000'
        surl: 'https://dtt.local:3001'
        whitelisted_paths: whitelisted_paths
    api:
        host: 'api.dtt.local'
        port: 3001
        client_ID: '1234'
        client_secret: 'secret'
        url: 'https://api.dtt.local:3001'
        whitelisted_paths: whitelisted_paths
    redis:
        host:'api.dtt.local'
        port: 6380
        password: 'welc0me'
    email:
        user: 'AKIAJH4LGM6D7JTFT4MQ'
        password: 'AlFd3CC8EzPTn+GE301M9oQthfZU5aPPBmKemJtz4e4X'
    github:#FOR TEST PURPOSES, TODO: ask John for the real deal
        client_id: '238dc978aaf2621d38b5'
        secret: 'f86f03ae61ed557e0bb97cfbc25c5d0e43f0a350'
        redirect_url: 'http://api.dtt.local:3000/github/auth/callback'
        state: 'zY6KPiIcKuhTYOdoUSX8avKc2mGASfNfHkvP50nAkPo='
    plugins:
        visual_studio:
            manifest: visual_studio_plugin_json
            installer: visual_studio_plugin_installer
    log:
        path: path.join(process.cwd(), '..', '..', 'development_logs', 'test.log')
        token: ''
        logkey: 'eespinal'
        apikey: '38af1eb61bad44a0b8ca6db044d672ff'
        

production_settings =
    env: 'production'
    db:
        connection_string: "postgres://huyuuxyveqegxe:tMU5vspNvcoPxePlBbK5DX1Jvx@ec2-23-21-91-108.compute-1.amazonaws.com:5432/d9er2dp9rejk7k"
    site:
        host: 'yacketyapp.com'
        port: 80
        client_ID: '1234'
        client_secret: 'secret'
        url: 'http://yacketyapp.com'
        surl: 'https://yacketyapp.com'
        whitelisted_paths: whitelisted_paths
    api:
        host: 'api.yacketyapp.com'
        port: 443
        client_ID: '1234'
        client_secret: 'secret'
        url: 'https://api.yacketyapp.com'
        whitelisted_paths: whitelisted_paths
    redis:
        host: 'api.yacketyapp.com'
        port: 6379
        password: '15439fde8d415f7ab4a3cc9b389badea'
    email:
        user: 'AKIAJH4LGM6D7JTFT4MQ'
        password: 'AlFd3CC8EzPTn+GE301M9oQthfZU5aPPBmKemJtz4e4X'
    github:#FOR TEST PURPOSES, TODO: ask John for the real deal
        client_id: 'cfb1bc4d1ed5dc9199bf'
        secret: '404f517c7c588165277fe3d1550360a77d1d388e'
        redirect_url: 'http://api.yacketyapp.com/github/auth/callback'
        state: 'zY6KPiIcKuhTYOdoUSX8avKc2mGASfNfHkvP50nAkPo='
    plugins:
        visual_studio:
            manifest: visual_studio_plugin_json
            installer: visual_studio_plugin_installer
    log:
        path: path.join('/var', 'log', 'yackety.log')
        token: 'dbc2a3a0-2801-4ab9-8009-f01dd3ac7706'
        logkey: 'eespinal'
        apikey: '38af1eb61bad44a0b8ca6db044d672ff'


module.exports = ->
    switch process.env.NODE_ENV
        when 'test' then test_settings
        when 'production' then production_settings
        else development_settings

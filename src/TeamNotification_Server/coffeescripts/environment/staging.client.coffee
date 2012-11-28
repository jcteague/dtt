define 'config', [], ->

    settings =
        whitelist: ['/', '/user/login','/user/login/', '/registration','/registration/']
        site:
            host: 'stagingdtt.local'
            port: 443
            url: 'https://stagingdtt.local'
        api:
            host: 'api.stagingdtt.local'
            port: 443
            url: 'https://stagingdtt.local/api'

    settings

define 'config', [], ->

    settings =
        whitelist: ['/', '/user/login','/user/login/', '/registration','/registration/']
        site:
            host: 'dtt.local'
            port: 443
            url: 'https://dtt.local'
        api:
            host: 'api.dtt.local'
            port: 443
            url: 'https://dtt.local/api'

    settings

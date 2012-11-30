define 'config', [], ->

    settings =
        whitelist: ['/user/login','/reset_password','/forgot_password','/registration']
        site:
            host: 'dtt.local'
            port: 443
            url: 'https://dtt.local'
        api:
            host: 'api.dtt.local'
            port: 443
            url: 'https://dtt.local/api'

    settings

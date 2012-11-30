define 'config', [], ->

    settings =
        whitelist: ['/', '/user/login', '/user/login/','/reset_password','/reset_password/','/forgot_password','/forgot_password/', '/registration','/registration/']
        site:
            host: 'dtt.local'
            port: 443
            url: 'https://dtt.local'
        api:
            host: 'api.dtt.local'
            port: 443
            url: 'https://dtt.local/api'

    settings

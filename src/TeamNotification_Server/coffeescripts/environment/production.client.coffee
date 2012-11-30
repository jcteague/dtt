define 'config', [], ->

    settings =
        whitelist: ['/', '/user/login', '/user/login/','/reset_password','/reset_password/','/forgot_password','/forgot_password/', '/registration','/registration/']
        site:
            host: 'yacketyapp.com'
            port: 443
            url: 'https://yacketyapp.com'
        api:
            host: 'api.yacketyapp.com'
            port: 443
            url: 'https://yacketyapp.com/api'

    settings

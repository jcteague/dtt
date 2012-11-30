define 'config', [], ->

    settings =
        whitelist: ['/user/login','/reset_password','/forgot_password','/registration']
        site:
            host: 'yacketyapp.com'
            port: 443
            url: 'https://yacketyapp.com'
        api:
            host: 'api.yacketyapp.com'
            port: 443
            url: 'https://yacketyapp.com/api'

    settings

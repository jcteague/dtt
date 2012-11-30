define 'config', [], ->

    settings =
        whitelist: ['/', '/user/login', '/user/login/','/reset_password','/reset_password/','/forgot_password','/forgot_password/', '/registration','/registration/']
        site:
            host: 'stagingdtt.local'
            port: 443
            url: 'https://stagingdtt.local'
        api:
            host: 'api.stagingdtt.local'
            port: 443
            url: 'https://stagingdtt.local/api'

    settings

define 'config', [], ->

    settings =
        whitelist: ['/user/login','/reset_password','/forgot_password','/registration']
        site:
            host: 'stagingdtt.local'
            port: 443
            url: 'https://stagingdtt.local'
        api:
            host: 'api.stagingdtt.local'
            port: 443
            url: 'https://stagingdtt.local/api'

    settings

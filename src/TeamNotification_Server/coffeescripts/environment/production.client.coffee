define 'config', [], ->

    settings =
        site:
            host: 'yacketyapp.com'
            port: 443
            url: 'https://yacketyapp.com'
        api:
            host: 'api.yacketyapp.com'
            port: 443
            url: 'https://yacketyapp.com/api'

    settings

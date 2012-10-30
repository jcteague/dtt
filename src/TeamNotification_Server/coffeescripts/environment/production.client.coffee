define 'config', [], ->

    settings =
        site:
            host: 'yacketyapp.com'
            port: 80
            url: 'http://yacketyapp.com'
        api:
            host: 'api.yacketyapp.com'
            port: 443
            url: 'https://api.yacketyapp.com'

    settings

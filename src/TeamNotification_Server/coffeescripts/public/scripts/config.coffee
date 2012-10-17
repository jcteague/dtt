define 'config', [], ->

    production_settings =
        site:
            host: 'yacketyapp.com'
            port: 80
            url: 'http://yacketyapp.com'
        api:
            host: 'api.yacketyapp.com'
            port: 443
            url: 'https://api.yacketyapp.com'

    development_settings =
        site:
            host: 'dtt.local'
            port: 3000
            url: 'http://dtt.local:3000'
        api:
            host: 'api.dtt.local'
            port: 3001
            url: 'https://api.dtt.local:3001'

    if has('production')
        return production_settings
    else
        return development_settings

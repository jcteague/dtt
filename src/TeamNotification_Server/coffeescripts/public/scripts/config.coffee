define 'config', [], ->

    production_settings =
        site:
            host: 'yacketyapp.com'
            port: 80
            url: 'http://yacketyapp.com'

    development_settings =
        site:
            host: 'dtt.local'
            port: 3000
            url: 'http://dtt.local:3000'

    if has('production')
        return production_settings
    else
        return development_settings

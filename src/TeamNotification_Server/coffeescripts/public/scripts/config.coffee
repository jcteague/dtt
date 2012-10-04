define 'config', [], ->

    production_settings =
        site:
            host: '54.243.207.101'
            port: 80
            url: 'http://54.243.207.101'

    development_settings =
        site:
            host: 'dtt.local'
            port: 3000
            url: 'http://dtt.local:3000'

    if has('production')
        return production_settings
    else
        return development_settings

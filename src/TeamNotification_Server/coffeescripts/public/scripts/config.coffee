define 'config', [], ->

    production_settings =
        site:
            host: 'dtt.jit.su'
            port: 80
            url: 'http://dtt.jit.su'

    development_settings =
        site:
            host: 'localhost'
            port: 3000
            url: 'http://dtt.local:3000'

    if has('production')
        return production_settings
    else
        return development_settings

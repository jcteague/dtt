define 'config', [], ->

    production_settings =
        site:
            host: 'ec2-107-21-171-44.compute-1.amazonaws.com'
            port: 80
            url: 'http://ec2-107-21-171-44.compute-1.amazonaws.com'

    development_settings =
        site:
            host: 'dtt.local'
            port: 3000
            url: 'http://dtt.local:3000'

    if has('production')
        return production_settings
    else
        return development_settings

define 'config', [], ->

    production_settings =
        site:
            host: 'ec2-107-21-171-44.compute-1.amazonaws.com'
            port: 80
            url: 'http://ec2-107-21-171-44.compute-1.amazonaws.com'

    development_settings =
        site:
            host: 'localhost'
            port: 3000
            url: 'http://localhost:3000'

    if has('production')
        return production_settings
    else
        return development_settings

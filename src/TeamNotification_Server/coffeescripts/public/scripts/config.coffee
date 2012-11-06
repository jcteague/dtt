define 'config', [], ->

    settings =
        site:
            host: 'dtt.local'
            port: 443
            url: 'https://dtt.local'
        api:
            host: 'api.dtt.local'
            port: 443
            url: 'https://dtt.local/api'

    settings

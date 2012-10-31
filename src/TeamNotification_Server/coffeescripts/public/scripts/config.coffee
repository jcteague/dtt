define 'config', [], ->

    production_settings =
        site:
            host: 'yacketyapp.com'
            port: 80
            url: 'http://yacketyapp.com'
        api:
            host: 'api.yacketyapp.com'
            port: 443
            url: 'https://api.yacketyapp.com/api'

    development_settings =
        site:
            host: 'dtt.local'
            port: 3000
            url: 'http://dtt.local:3000'
        api:
            host: 'api.dtt.local'
            port: 3001
            url: 'https://dtt.local:3001/api'

    if has('production')
        return production_settings
    else
        # Need explicit support in some local clients.
        # TODO: Remove when handling CORS correctly
        if jQuery.browser?.mozilla
            jQuery.support.cors = true

        return development_settings

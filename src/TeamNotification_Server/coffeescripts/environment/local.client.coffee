define 'config', [], ->

    settings =
        site:
            host: 'dtt.local'
            port: 3000
            url: 'http://dtt.local:3000'
        api:
            host: 'api.dtt.local'
            port: 3001
            url: 'https://api.dtt.local:3001'
            
    # Need explicit support in some local clients.
    # TODO: Remove when handling CORS correctly
    if jQuery.browser?.mozilla
        jQuery.support.cors = true

    settings

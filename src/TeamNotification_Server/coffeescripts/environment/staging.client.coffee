define 'config', [], ->

    settings =
        site:
            host: 'staging.dtt.local'
            port: 80
            url: 'http://staging.dtt.local'
        api:
            host: 'api.staging.dtt.local'
            port: 443
            url: 'https://api.staging.dtt.local'

    # Need explicit support in some local clients.
    # TODO: Remove when handling CORS correctly
    if jQuery.browser?.mozilla
        jQuery.support.cors = true

    settings

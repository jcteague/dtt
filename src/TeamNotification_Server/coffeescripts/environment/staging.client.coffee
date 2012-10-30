define 'config', [], ->

    settings =
        site:
            host: 'stagingdtt.local'
            port: 80
            url: 'http://stagingdtt.local'
        api:
            host: 'api.stagingdtt.local'
            port: 443
            url: 'https://api.stagingdtt.local'

    # Need explicit support in some local clients.
    # TODO: Remove when handling CORS correctly
    if jQuery.browser?.mozilla
        jQuery.support.cors = true

    settings

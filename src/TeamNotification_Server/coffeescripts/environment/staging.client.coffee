define 'config', [], ->

    settings =
        site:
            host: 'stagingdtt.local'
            port: 443
            url: 'https://stagingdtt.local'
        api:
            host: 'api.stagingdtt.local'
            port: 443
            url: 'https://stagingdtt.local/api'

    settings

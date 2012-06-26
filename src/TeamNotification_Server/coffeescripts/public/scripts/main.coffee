require.config
    baseUrl: 'scripts'
    paths:
        'jquery': 'jquery-1.7.2.min'
        'underscore': 'underscore-min'
        'backbone': 'backbone-min'
        'jquery.autocomplete': 'jquery.autocomplete'
        'client_view': 'client_view'
        'client_router': 'client_router'
        'form_template_renderer': 'form_template_renderer'
        'query_renderer': 'query_renderer'

    shim:
        'backbone':
            deps: ['underscore', 'jquery']
            exports: 'Backbone'

        'jquery.autocomplete':
            deps: ['jquery']

require ['jquery', 'client_view'], ($, ClientView) ->
    $ ->
        new ClientView().render()

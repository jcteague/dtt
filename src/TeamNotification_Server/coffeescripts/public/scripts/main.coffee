require.config
    baseUrl: 'scripts'
    paths:
        'jquery': 'jquery-1.7.2.min'
        'underscore': 'underscore-min'
        'backbone': 'backbone-min'
        'jquery.autocomplete': 'jquery.autocomplete'
        'jquery.validate': 'jquery.validate.min'
        'jquery-ui': 'jquery-ui.min'
        'config': 'config'
        'client_view': 'client_view'
        'client_router': 'client_router'
        'form_view': 'form_view'
        'links_view': 'links_view'
        'query_view': 'query_view'
        'server_response_view': 'server_response_view'
        'views_factory': 'views_factory'
        'messages_view': 'messages_view'
        'rooms_view': 'rooms_view'
        'form_template_renderer': 'form_template_renderer'
        'query_renderer': 'query_renderer'
        'collection_model': 'collection_model'
        'bootstrap': 'bootstrap.min'
        'bootstrap-dropdown': 'bootstrap-dropdown'
        'bootstrap-collapse': 'bootstrap-collapse'
        'prettify': 'prettify/prettify'
        'lang-go': 'prettify/lang-go'
        'lang-hs': 'prettify/lang-hs'
        'lang-lisp': 'prettify/lang-lisp'
        'lang-ml': 'prettify/lang-ml'
        'lang-proto': 'prettify/lang-proto'
        'lang-scala': 'prettify/lang-scala'
        'lang-vhdl': 'prettify/lang-vhdl'

    shim:
        'prettify' :
            deps: ['lang-go','lang-hs','lang-lisp','lang-ml','lang-proto','lang-scala','lang-vhdl']

        'backbone':
            deps: ['underscore', 'jquery']
            exports: 'Backbone'

        'jquery.autocomplete':
            deps: ['jquery']

        'jquery-ui':
            deps: ['jquery']

        'jquery.validate':
            deps: ['jquery']

require ['jquery', 'client_view'], ($, ClientView) ->
    $ ->
        new ClientView().render()

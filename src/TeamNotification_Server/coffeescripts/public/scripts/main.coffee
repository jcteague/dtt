require.config
    baseUrl: 'scripts'
    paths:
        'jquery': 'jquery-1.7.2.min'
        'base64': 'base64'
        'cookie': 'jquery.cookie'
        'underscore': 'underscore-min'
        'backbone': 'backbone-min'
        'moment': 'moment.min'
        'jquery.autocomplete': 'jquery.autocomplete'
        'jquery.validate': 'jquery.validate.min'
        'jquery.ie.cors': 'jquery.ie.cors'
        'config': 'config'
        'client_view': 'client_view'
        'user_edit_view': 'user_edit_view'
        'login_view': 'login_view'
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
        'prettify-languages': 'prettify-languages'
        'lang-go': 'prettify/lang-go'
        'lang-hs': 'prettify/lang-hs'
        'lang-lisp': 'prettify/lang-lisp'
        'lang-ml': 'prettify/lang-ml'
        'lang-proto': 'prettify/lang-proto'
        'lang-scala': 'prettify/lang-scala'
        'lang-vhdl': 'prettify/lang-vhdl'

    shim:
        'prettify-languages':
            deps: ['prettify']

        'backbone':
            deps: ['underscore', 'jquery']
            exports: 'Backbone'

        'jquery.autocomplete':
            deps: ['jquery']
        'cookie':
            deps: ['jquery']
        'base64':
            deps: ['jquery']

        'jquery.validate':
            deps: ['jquery']

require ['jquery', 'client_view'], ($, ClientView) ->
    $ ->
        new ClientView().render()

require.config
    baseUrl: 'scripts'
    paths:
        'jquery': 'jquery-1.7.2.min'
        'underscore': 'underscore-min'
        'backbone': 'backbone-min'
        'form_renderer': 'FormRenderer'

    shim:
        'backbone':
            deps: ['underscore', 'jquery']
            exports: 'Backbone'
        

require ["jquery"], ($) ->
    $ ->
        alert('hello')

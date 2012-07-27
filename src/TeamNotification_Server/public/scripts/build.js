({
    name: 'main',
    out: '../../../../deploy/public/scripts/main.js',
    has: {
        production: true
    },
    baseUrl: '.',
    paths: {
      'jquery': 'jquery-1.7.2.min',
      'underscore': 'underscore-min',
      'backbone': 'backbone-min',
      'jquery.autocomplete': 'jquery.autocomplete',
      'jquery.validate': 'jquery.validate.min',
      'config': 'config',
      'client_view': 'client_view',
      'client_router': 'client_router',
      'form_view': 'form_view',
      'links_view': 'links_view',
      'query_view': 'query_view',
      'server_response_view': 'server_response_view',
      'views_factory': 'views_factory',
      'messages_view': 'messages_view',
      'rooms_view': 'rooms_view',
      'form_template_renderer': 'form_template_renderer',
      'query_renderer': 'query_renderer',
      'collection_model': 'collection_model'
    },
    shim: {
      'backbone': {
        deps: ['underscore', 'jquery'],
        exports: 'Backbone'
      },
      'jquery.autocomplete': {
        deps: ['jquery']
      },
      'jquery.validate': {
        deps: ['jquery']
      }
    }
})

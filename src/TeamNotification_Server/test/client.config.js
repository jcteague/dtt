var path = require('path');
var config = {
    //nodeRequire: require
    baseUrl: path.resolve(__dirname, '..', 'public', 'scripts'),
    paths: {
      'underscore': 'underscore-min',
      'backbone': 'backbone-min',
      'client_view': 'client_view',
      'client_router': 'client_router',
      'form_template_renderer': 'form_template_renderer'
    },
    shim: {
      'backbone': {
        deps: ['underscore', 'jquery'],
        exports: 'Backbone'
      }
    }
};

module.exports = config

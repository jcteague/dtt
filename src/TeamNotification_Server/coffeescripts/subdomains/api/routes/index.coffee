module.exports = (app, io) ->
    routes = require('requireindex')(__dirname)
    routes[key].build_routes?(app, io) for key, value of routes

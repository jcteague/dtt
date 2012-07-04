methods = {}
methods.authorize = () ->

methods.get_token = () ->


module.exports =
    methods: methods
    build_routes: (app) ->
        app.get '/oauth2/authorize', methods.authorize
        app.get '/oauth2/token', methods.get_token

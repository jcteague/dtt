methods = {}
methods.authorize = (req, res) ->
    console.log 'get', req.query, req.params, req.body
    model =
        client_id: req.param('client_id')
        redirect_uri: req.param('redirect_uri')
    res.render('authorize.jade', model: model)

methods.post_authorize = (req, res) ->
    console.log 'post', req.query, req.params, req.body

methods.get_token = (req, res) ->
    console.log 'token', req.query

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get '/oauth2/authorize', methods.authorize
        app.post '/oauth2/authorize', methods.post_authorize
        app.get '/oauth2/token', methods.get_token

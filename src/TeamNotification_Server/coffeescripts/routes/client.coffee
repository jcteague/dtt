methods = {}
methods.get_client = (req, res) ->
    res.render('client.jade', {})

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/client', methods.get_client)

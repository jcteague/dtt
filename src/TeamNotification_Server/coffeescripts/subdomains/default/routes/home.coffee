methods = {}
methods.get_client = (req, res) ->
    console.log 'ON THE CLIENT'
    res.render('client.jade', {})

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/', methods.get_client)

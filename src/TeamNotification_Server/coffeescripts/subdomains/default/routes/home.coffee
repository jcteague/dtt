config = require('../../../config')()

methods = {}
methods.get_client = (req, res) ->
    res.render('client.jade', model: {api: config.api.url})

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/', methods.get_client)

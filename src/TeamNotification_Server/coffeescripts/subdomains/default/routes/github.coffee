config = require('../../../config')()
methods = {}

methods.github_redirect = (req, res) ->
    console.log 'here'
    res.redirect("https://github.com/login/oauth/authorize?client_id=#{config.github.client_id}&scope=user,repo&state=#{config.github.state}")

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/github/oauth', methods.github_redirect)

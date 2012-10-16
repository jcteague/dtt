http = require('https')
querystring = require('querystring')
config = require('../../../config')()
methods = {}

methods.github_redirect = (req, res) ->
    res.redirect("https://github.com/login/oauth/authorize?client_id=#{config.github.client_id}&scope=user,repo&state=#{config.github.state}")

methods.github_authentication_callback = (req, res) ->
    code = req.query.code
    post_fields =
        'client_id' : config.github.client_id
        'client_secret': config.github.secret
        'code': code
        'state': config.github.state
    post_data = querystring.stringify( post_fields)
    post_options =
        host: 'github.com'
        port: 443
        method: 'POST'
        path: '/login/oauth/access_token'
        headers:
            'Accept': 'application/json'
            'Content-Type': 'application/x-www-form-urlencoded'
            'Content-Length': post_data.length
    post_req = http.request post_options, (post_res) ->
        post_res.setEncoding('utf8')
        post_res.on 'data', (chunk) ->
            data = JSON.parse(chunk)
            if(typeof(data.access_token) != undefined)
                res.redirect("#{config.site.url}/#/github/repositories/#{data.access_token}")
            else
                res.send({success:false, messages:['There was a problem contacting github']})
        post_res.on 'error', (error) ->
            console.log("Got error: " + error.message)
            res.send({success:false, messages:[error.message]})
    post_req.end(post_data)

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/github/oauth', methods.github_redirect)
        app.get('/github/auth/callback', methods.github_authentication_callback)

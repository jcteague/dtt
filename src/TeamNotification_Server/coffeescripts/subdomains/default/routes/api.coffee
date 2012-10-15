http = require('http')
querystring = require('querystring')
config = require('../../../config')()

make_callback = (res) ->
    return (response) ->
      response.setEncoding('utf8')
      str = ''

      response.on 'data', (chunk) ->
          str += chunk

      response.on 'end', () ->
          console.log(str)
          res.send str

get_cross_subdomain_path_from = (path) ->
  path.split('/api').slice(1).join('')

methods = {}

methods.get_api = (req, res) ->
    console.log 'GET API URL', req.url
    options = 
        host: "api.#{config.site.host}"
        path: get_cross_subdomain_path_from req.url
        port: config.site.port

    http.request(options, make_callback(res)).end()

methods.post_api = (req, res) ->
    console.log 'POST API URL', req.url
    console.log req.body
    post_data = querystring.stringify req.body
    options = 
        host: "api.#{config.site.host}"
        path: get_cross_subdomain_path_from req.url
        port: config.site.port
        method: 'POST'
        headers:
          'Content-Type': 'application/x-www-form-urlencoded',
          'Content-Length': post_data.length

    request = http.request(options, make_callback(res))
    request.write post_data
    request.end()

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/api', methods.get_api)
        app.get('/api/*', methods.get_api)
        app.post('/api', methods.post_api)
        app.post('/api/*', methods.post_api)

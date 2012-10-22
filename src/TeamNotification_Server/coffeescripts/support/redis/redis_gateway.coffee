config = require('../../config')().redis

redis = require("redis")
#redis.debug_mode = true

get_connection = ->
    client = redis.createClient(config.port, config.host)
    client

module.exports =
    open: get_connection

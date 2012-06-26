q = require('q')
pg = require('pg')
db_config = require('../globals').db
connection_string = "tcp://#{db_config.user}:#{db_config.password}@#{db_config.host}"
connection_string = "postgres://#{db_config.user}:#{db_config.password}@#{db_config.host}"

get_db_connection = (database) ->
    defer = q.defer()
    pg.connect "#{connection_string}/#{database}", (err, client) ->
        if err
            console.log "Could not connect to POSTGRES '#{database}' database"
            console.log err
            return
        defer.resolve client

    defer.promise

module.exports =
    open: get_db_connection

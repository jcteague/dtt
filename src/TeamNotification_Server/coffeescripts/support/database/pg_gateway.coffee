q = require('q')
pg = require('pg')
db_config = require('../../config').db
connection_string = "postgres://#{db_config.user}:#{db_config.password}@#{db_config.host}"

open_normal_connection = (callback) ->
    pg.connect db_config.connection_string, (err, client) ->
        if err
            console.log "Could not connect to POSTGRES '#{database}' database"
            console.log err
            return

        callback(client)

module.exports =
    open: open_normal_connection

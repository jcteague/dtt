q = require('q')
pg = require('pg')
db_config = require('../../config')().db

open_normal_connection = (callback) ->
    pg.connect db_config.connection_string, (err, client) ->
        if err
            console.log "Could not connect to POSTGRES database using #{db_config.connection_string}"
            console.log err
            return

        callback(client)

module.exports =
    open: open_normal_connection

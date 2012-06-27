q = require('q')
orm = require('orm')

db_config = require('../config').db

get_db_connection = ->
    defer = q.defer()
    orm.connect db_config.connection_string, (success, db) ->
        unless success
            console.log "Could not connect to #{db_config.connection_string}"
            return

        defer.resolve(db)

    defer.promise

module.exports =
    open: get_db_connection

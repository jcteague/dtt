q = require('q')
orm = require('orm')

db_config = require('./globals').db

get_db_connection = (database) ->
    defer = q.defer()
    orm.connect db_config.get_connection_string_for(database), (success, db) ->
        unless success
            console.log "Could not connect to #{database} database"
            return

        defer.resolve(db)

    defer.promise

module.exports =
    open: get_db_connection

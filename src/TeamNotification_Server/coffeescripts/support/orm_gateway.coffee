q = require('q')
pg = require('pg')
orm = require('orm')
logger = require('./logging/logger')

db_config = require('../config')().db

get_db_connection = ->
    client = new pg.Client(db_config.connection_string)
    client.connect()
    defer = q.defer()
    orm.connect 'postgres', client, (success, db) ->
        unless success
            logger.error "Could not connect to #{db_config.connection_string}"
            return

        defer.resolve(db)

    defer.promise

module.exports =
    open: get_db_connection

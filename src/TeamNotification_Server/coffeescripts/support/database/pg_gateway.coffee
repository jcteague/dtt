q = require('q')
pg = require('pg')
db_config = require('../../config')().db
logger = require('../logging/logger')

open_normal_connection = (callback) ->
    pg.connect db_config.connection_string, (err, client) ->
        if err
            logger.error "Could not connect to POSTGRES database using #{db_config.connection_string}.", {error: err}
            return

        callback(client)

open_promise = ->
    deferred = q.defer()
    pg.connect db_config.connection_string, (err, client) ->
        if err
            logger.error "Could not connect to POSTGRES database using #{db_config.connection_string}.", {error: err}
            return

        q.resolve client

    deferred.promise

module.exports =
    open: open_normal_connection
    open_promise: open_promise

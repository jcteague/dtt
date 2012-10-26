async = require('async')

db = require('./db_helper')
entities = require('./predefined_db_entities')
server = require('./server_helper')
config = require('../../config')()

handle_in_series = (steps..., done) ->
    async.series steps, () ->
        done()

module.exports =
    db: db
    entities: entities
    server: server
    handle_in_series: handle_in_series
    config: config

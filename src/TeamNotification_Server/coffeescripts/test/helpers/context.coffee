Q = require('q')
pg_gateway = require('../../support/database/pg_gateway')
config = require('../../config')()

unit_test = (path, mocks) ->
    sut = ''

    return (block) ->

        describe 'This is a unit test context', ->
            block(sut, dependencies)

truncate_all_tables = ->
    binded_client = null
    pg_gateway.open_promise().
        then((client) -> 
            Q.nbind(client.query, client)
        ).then((b_client) ->
            binded_client = b_client
            binded_client("SELECT tablename FROM pg_tables WHERE tableowner = '#{config.db.user}' AND schemaname = 'public';")
        ).then((result) ->
            (row.tablename for row in result.rows when row.tablename isnt 'schema_migrations')
        ).then((tables) ->
            queries = ("TRUNCATE TABLE #{table};" for table in tables)
            binded_client(queries.join(' '))
        ).then -> null

integration_test = (block) ->

    beforeEach (done) ->
        truncate_all_tables().then(done)

    describe '', ->
        block()

module.exports =
    for:
        unit_test: unit_test
        integration_test: integration_test

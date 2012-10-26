Q = require('q')
pg_gateway = require('../../support/database/pg_gateway')

using = (client) ->
    Q.nbind(client.query, client)

using_client = (client) ->
    return (queries_array) ->
        reduce_func = (processed_queries, next_query) ->
            processed_queries.then(client).then(next_query)

        queries_array.reduce reduce_func, Q.resolve(client)

handle = (client, query) ->
    Q.ncall(client.query, client)

handle_queries = (queries_array) ->
    return (client) ->
        reduce_func = (processed_queries, next_query) ->
            processed_queries.then(client).then(next_query)

        queries_array.reduce reduce_func, Q.resolve(client)

unit_test = (path, mocks) ->
    sut = ''

    return (block) ->

        describe 'This is a unit test context', ->
            block(sut, dependencies)

###
truncate_all_tables = ->
    console.log 'TRUNCATING ALL'
    pg_gateway.open_promise().
        then((client) ->
            Q.ncall(client.query, client, "TRUNCATE TABLE #{table}").then(client)
        ).then((client) ->
            Q.ncall(client.query, client, "TRUNCATE TABLE #{table}").then(client)
        )
###

truncate_all_tables = ->
    console.log 'TRUNCATING ALL'
    queries = ['TRUNCATE', 'TRUNCATE']
    pg_gateway.open_promise().
        then((client) -> 
            Q.ncall(client.query, client, "SELECT tablename FROM pg_tables WHERE tableowner = #{user} AND schemaname = 'public';").then((result) -> client: client, result: result)).
        then(handle_queries queries)

integration_test = (block) ->

    beforeEach (done) ->
        truncate_all_tables().then(done)

    describe 'This is an integration test context', ->
        block()


methods.export =
    for:
        unit_test: unit_test
        integration_test: integration_test

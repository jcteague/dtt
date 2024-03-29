Q = require('q')
pg_gateway = require('../../support/database/pg_gateway')
config = require('../../config')()
_ = require('underscore')

Browser = require('zombie').Browser

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
            queries = ("TRUNCATE TABLE #{table} RESTART IDENTITY CASCADE;" for table in tables)
            binded_client(queries.join(' '))
        ).then -> null

get_auth_token = (email, password) ->
    "Basic " + (new Buffer(email + ":" + password).toString('base64'))

integration_test = (options) ->

    default_options =
        authenticate: true
        authenticated_user:
            email: 'foo@bar.com'
            password: '1234'
        log_errors: false
        browsers: 1

    opt = _.extend(default_options, options)
    console.log opt
    return (block) ->
        browsers = (new Browser({waitFor:0.7}) for i in [0...opt.browsers])

        beforeEach (done) ->
            for browser in browsers
                if opt.authenticate
                    email = opt.authenticated_user.email
                    password = opt.authenticated_user.password
                    browser.authenticate().basic(email, password)
                    auth_token = get_auth_token(email, password)
                    browser.cookies(config.site.host, '/').set('authtoken', auth_token)

                if opt.log_errors
                    browser.on 'error', (error) ->
                        console.log 'Browser', error

            truncate_all_tables().then(done)

        describe '', ->
            block.apply(null, browsers)

module.exports =
    for:
        unit_test: unit_test
        integration_test: integration_test

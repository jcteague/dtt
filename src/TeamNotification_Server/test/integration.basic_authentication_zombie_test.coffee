expect = require('expect.js')
sinon = require('sinon')
context = require('./helpers/context')
{db: db, entities: entities, server: server, handle_in_series: handle_in_series, config: config} = require('./helpers/specs_helper')

users =
    name: 'users'
    entities: [
        {
            id: 1
            first_name: "'blah'"
            last_name: "'bar'"
            email: "'foo@bar.com'"
            password: "'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'"
        }
    ]


context.for.integration_test(authenticate: false) (browser) ->

    describe 'Basic Authentication', ->

        beforeEach (done) ->
            handle_in_series server.start(), db.save(users), done

        describe 'When an unauthenticated request is received', ->

            response = null
            login_link = '<a href="#/user/login">Login</a>'
            beforeEach (done) ->
                browser.cookies().clear()
                browser.visit "#{config.site.surl}/#/user/1", (e, browser, status) ->
                    response = status
                    done()

            it 'should redirect to the main page', (done) ->
                expect(browser.url).to.eql config.site.surl+'/#/'
                done()

        describe 'When an authenticated header is found', ->

            response = null

            beforeEach (done) ->
                browser.authenticate().basic('foo@bar.com', '1234')
                browser.visit "#{config.site.surl}", (e, browser, status) ->
                    response = status
                    done()

            it 'should return a 200 status code', (done) ->
                expect(response).to.equal 200
                done()

# tobi package is broken in latest node
return
tobi = require('tobi')
should = require('should')
expect = require('expect.js')
sinon = require('sinon')
{db: db, entities: entities, server: server, handle_in_series: handle_in_series} = require('./helpers/specs_helper')

module_loader = require('sandboxed-module')

users =
    name: 'users'
    entities: [
        {
            id: 1
            first_name: "'blah'"
            last_name: "'bar'"
            email: "'foo'"
            password: "'password'"
        }
    ]

describe 'Basic Authentication Test', ->

    browser = null

    beforeEach (done) ->
        handle_in_series server.start(), db.clear('users'), db.create(entities.users), db.save(users), done

    describe 'When an unauthenticated request is received', ->

        response = null

        beforeEach (done) ->
            browser = tobi.createBrowser(3000, 'localhost')
            browser.get '/', (res, $) ->
                response = res
                done()

        it 'should return a 401', (done) ->
            response.should.have.status 401
            done()

    describe 'When an authenticated header is found', ->

        response = null

        beforeEach (done) ->
            browser = tobi.createBrowser(3000, 'localhost')
            #foo:password must be sent encoded as the specification
            browser.get '/', {headers:{"authorization": "Basic Zm9vOnBhc3N3b3Jk"}}, (res, $) ->
                response = res
                done()

        it 'should return a 200 status code', (done) ->
            response.should.have.status 200
            done()

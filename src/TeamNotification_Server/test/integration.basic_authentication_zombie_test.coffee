expect = require('expect.js')
sinon = require('sinon')
expect = require('expect.js')
{db: db, entities: entities, server: server, handle_in_series: handle_in_series} = require('./helpers/specs_helper')

module_loader = require('sandboxed-module')
Browser = require('zombie').Browser

users =
    name: 'users'
    entities: [
        {
            id: 1
            name: "'blah'"
            email: "'foo@bar.com'"
            password: "'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'"
        }
    ]

describe 'Basic Authentication Test', ->

    browser = null

    beforeEach (done) ->
        browser = new Browser()
        handle_in_series server.start(), db.clear('users'), db.create(entities.users), db.save(users), done

    describe 'When an unauthenticated request is received', ->

        response = null

        beforeEach (done) ->
            browser.visit 'http://localhost:3000/', (e, browser, status) ->
                response = status
                done()

        it 'should return a 401', (done) ->
            expect(response).to.equal 401
            done()

    describe 'When an authenticated header is found', ->

        response = null

        beforeEach (done) ->
            browser.authenticate().basic('foo@bar.com', '1234')
            browser.visit 'http://localhost:3000/', (e, browser, status) ->
                response = status
                done()

        it 'should return a 200 status code', (done) ->
            expect(response).to.equal 200
            done()

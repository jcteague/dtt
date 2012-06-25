return
expect = require('expect.js')
sinon = require('sinon')
test_helper = require('./helpers/database_helper')

entities =
    ChatRoom: [
        {id: 1, name: 'foo'}
        {id: 2, name: 'bar'}
    ]
    User: [
        {id: 1, name: 'blah', email: 'foo@bar.com'}
    ]

test_helper.set_up_db(entities)


module_loader = require('sandboxed-module')
Browser = require('zombie').Browser

describe 'Add Account To Chat Room', ->

    describe 'Set Up', ->

        beforeEach (done) ->
            test_helper.set_up_db(entities)
            done()

        afterEach (done) ->
            test_helper.clean_up_db(entities)
            done()

        browser = null

        beforeEach (done) ->
            browser = new Browser()
            done()

        describe 'When a user visits the client#/room/:id/users page', ->

            beforeEach (done) ->
                browser.
                    visit('http://localhost:3000/client#/room/1/users').
                    then(done, done)

            it 'should contain an autocomplete input', (done) ->
                expect(browser.html('input#username_autocomplete')).to.not.be.empty()
                done()

        ###
        describe 'When a user visits the client#/room/:id/users page', ->

            describe 'and fills in a username that exists in the system', ->

                beforeEach (done) ->
                    browser.
                        visit('http://localhost:3000/client#/room/1/users').
                        then(-> browser.fill('#username_autocomplete', 'blah').pressButton('#username_submit')).
                        then(done, done)

                it 'should ', (done) ->
                    expect().
                    done()
        ###

expect = require('expect.js')
sinon = require('sinon')
{db: db, entities: entities} = require('./helpers/specs_helper')

module_loader = require('sandboxed-module')
Browser = require('zombie').Browser

describe 'Add Account To Chat Room', ->

    describe 'Set Up', ->

        beforeEach (done) ->
            db.handle db.clear('users', 'chat_room'), db.create(entities.users, entities.chat_rooms), db.save(users, chat_rooms), done

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

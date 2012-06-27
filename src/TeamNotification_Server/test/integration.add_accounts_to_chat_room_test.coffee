return unless process.env.NODE_ENV is 'test'

expect = require('expect.js')
sinon = require('sinon')
{db: db, entities: entities} = require('./helpers/specs_helper')

module_loader = require('sandboxed-module')
Browser = require('zombie').Browser

users =
    name: 'users'
    entities: [
        {
            id: 1
            name: "'blah'"
            email: "'foo@bar.com'"
        },
        {
            id: 2
            name: "'ed2'"
            email: "'ed@es.com'"
        }
    ]

chat_rooms =
    name: 'chat_room'
    entities: [
        {
            id: 1
            name: "'foo'"
            owner_id: 1
        },
        {
            id: 2
            name: "'bar'"
            owner_id: 1
        }
    ]


describe 'Add Account To Chat Room', ->

    describe 'Set Up', ->

        browser = null

        beforeEach (done) ->
            browser = new Browser()
            db.handle db.clear('users', 'chat_room'), db.create(entities.users, entities.chat_rooms), db.save(users, chat_rooms), done

        describe 'When a user visits the client#/room/:id/users page', ->

            beforeEach (done) ->
                browser.
                    visit('http://localhost:3000/client#/room/1/users').
                    then(done, done)

            it 'should contain an autocomplete input', (done) ->
                expect(browser.html('input.acInput')).to.not.be.empty()
                done()

        describe 'When a user visits the client#/room/:id/users page', ->

            user_id = null

            describe 'and submits in a user that exists in the system', ->

                beforeEach (done) ->
                    user_id = 2
                    browser.
                        visit('http://localhost:3000/client#/room/1/users').
                        then(-> 
                            # Autocomplete is not friendly with zombie, must mock call
                            func = """
                                li = $('<li></li>');
                                li.data('value', '<span class="name">blah</span><span class="hidden">#{user_id}</span>');
                                li.data('data', {});
                                $('.acInput').data('autocompleter').selectItem(li);
                            """
                            browser.evaluate(func)
                        ).
                        then(-> browser.pressButton('input[type=submit]')).
                        then(done, done)

                it 'should get a user added to user response', (done) ->
                    expect(browser.lastResponse.body).to.equal "user #{user_id} added"
                    done()

            describe 'and submits in a user that does not exist in the system', ->

                beforeEach (done) ->
                    user_id = 10
                    browser.
                        visit('http://localhost:3000/client#/room/1/users').
                        then(-> 
                            # Autocomplete is not friendly with zombie, must mock call
                            func = """
                                li = $('<li></li>');
                                li.data('value', '<span class="name">blah</span><span class="hidden">#{user_id}</span>');
                                li.data('data', {});
                                $('.acInput').data('autocompleter').selectItem(li);
                            """
                            browser.evaluate(func)
                        ).
                        then(-> browser.pressButton('input[type=submit]')).
                        then(done, done)

                it 'should get a user added to user response', (done) ->
                    expect(browser.lastResponse.body).to.equal "user #{user_id} does not exist"
                    done()

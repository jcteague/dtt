expect = require('expect.js')
sinon = require('sinon')
{db: db, entities: entities, server: server, handle_in_series: handle_in_series} = require('./helpers/specs_helper')

module_loader = require('sandboxed-module')
Browser = require('zombie').Browser

users =
    name: 'users'
    entities: [
        {
            id: 1
            first_name: "'blah'"
            last_name: "'bar'"
            email: "'foo@bar.com'"
            password: "'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'"
        },
        {
            id: 2
            first_name: "'ed2'"
            last_name: "'ed 3'"
            email: "'ed@es.com'"
            password: "'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'"
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
            owner_id: 2
        }
    ]

chat_room_users =
    name: 'chat_room_users'
    entities: [
        {
            user_id: 1
            chat_room_id: 2
        }
    ]

describe 'Client Room', ->

    describe 'Set Up', ->

        browser = null

        beforeEach (done) ->
            browser = new Browser()
            browser.authenticate().basic('foo@bar.com', '1234')
            handle_in_series server.start(), db.clear('users', 'chat_room', 'chat_room_users'), db.create(entities.users, entities.chat_rooms, entities.chat_room_users), db.save(users, chat_rooms, chat_room_users), done

        describe 'When a user visits the #/room page and he is the owner of the room', ->

            beforeEach (done) ->
                browser.
                    visit('http://dtt.local:3000/#/room/1').
                    then(done, done)

            xit 'should contain an anchor to the room manage members', (done) ->
                expect(browser.html('a[href="#/room/1/users"]')).to.not.be.empty()
                done()

            xit 'should contain a anchor to the room messages', (done) ->
                expect(browser.html('a[href="#/room/1/messages"]')).to.not.be.empty()
                done()

        describe 'When a user visits the #/room page and he is the owner of the room', ->

            beforeEach (done) ->
                browser.
                    visit('http://dtt.local:3000/#/room/2').
                    then(done, done)

            xit 'should not contain an anchor to the room manage members', (done) ->
                expect(browser.html('a[href="#/room/2/users"]')).to.be.empty()
                done()

            xit 'should contain a anchor to the room messages', (done) ->
                expect(browser.html('a[href="#/room/2/messages"]')).to.not.be.empty()
                done()

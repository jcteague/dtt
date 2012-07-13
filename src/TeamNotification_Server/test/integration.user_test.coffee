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

describe 'User Room', ->

    describe 'Set Up', ->

        browser = null

        beforeEach (done) ->
            browser = new Browser()
            browser.authenticate().basic('foo@bar.com', '1234')
            handle_in_series server.start(), db.clear('users', 'chat_room', 'chat_room_users'), db.create(entities.users, entities.chat_rooms, entities.chat_room_users), db.save(users, chat_rooms, chat_room_users), done

        describe 'When a user visits the client#/user/1 page', ->

            beforeEach (done) ->
                browser.
                    visit('http://localhost:3000/client#/user/1').
                    then(done, done)

            it 'should contain an anchor for the user rooms and the create room', (done) ->
                expect(browser.html('#links h1')).to.equal('<h1>Links</h1>')
                expect(browser.html('#links a[href="#/user/1/rooms"]')).to.not.be.empty()
                expect(browser.html('#links a[href="#/room"]')).to.not.be.empty()
                done()

            it 'should contain an anchor for each room the user is in', (done) ->
                expect(browser.html('#rooms-container h1')).to.equal('<h1>User Rooms</h1>')
                expect(browser.html('a[href="#/room/1"]')).to.not.be.empty()
                expect(browser.html('a[href="#/room/2"]')).to.not.be.empty()
                done()

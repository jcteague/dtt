console.log 'Resend Room Invitations'
return

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
            email: "'bar@foo.org'"
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
            owner_id: 1
        }
    ]
chat_room_invitations =
    name: 'chat_room_invitation'
    entities: [
        {
            id: 1
            user_id: 1
            chat_room_id: 1
            email: "'bar@aol.com'"
            date: "'2012-09-20 12:12:10.494376'"
            accepted: 0
        },
        {
            id: 2
            user_id: 1
            chat_room_id: 2
            email: "'bar@aol.com'"
            date: "'2012-09-20 12:12:10.037034'"
            accepted: 0
        }
    ]
    
    
    
    
describe 'Resend Room Invitations', ->
    describe 'Set Up', ->
        browser = null
        beforeEach (done) ->
            browser = new Browser()
            browser.authenticate().basic('foo@bar.com', '1234')
            handle_in_series server.start(), db.clear('users', 'chat_room', 'chat_room_users','chat_room_invitation'), db.create(entities.users, entities.chat_rooms, entities.chat_room_users, entities.chat_room_invitation), db.save(users, chat_rooms),db.save(chat_room_invitations) , done
    

        describe 'When a user visits the #/room/:id/invitations page', ->

            beforeEach (done) ->
                browser.
                    visit('http://dtt.local:3000/#/room/1/invitations').
                    then(done, done)

            it 'should contain an autocomplete input', (done) ->
                expect(browser.html('#invitations-table tr')).not.to.be.empty()
                done()
        
        describe 'When a user clicks to resend an invitation', ->
            beforeEach (done) ->
                browser.
                    visit('http://dtt.local:3000/#/room/1/invitations').
                    then(-> browser.pressButton('button[type=submit]')).
                    then(done, done)
            
            it 'should show a message explaining the invitation has been sent', (done)->
                expect(browser.html("#server-response-container")).not.to.be.empty()
                done()


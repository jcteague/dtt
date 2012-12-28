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
            enabled:1
        },
        {
            id: 2
            first_name: "'ed2'"
            email: "'bar@foo.org'"
            password: "'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'"
            enabled:1
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
    
    
context.for.integration_test() (browser) ->    
    
    describe 'Resend Room Invitations', ->

        beforeEach (done) ->
            handle_in_series server.start(), db.save(users, chat_rooms), db.save(chat_room_invitations) , done

        describe 'When a user visits the #/user', ->#room/:id/invitations page', ->

            beforeEach (done) ->
                browser.
                    visit("#{config.site.surl}/#/user"). #room/1/invitations").
                    then(done, done)

            it 'should contain an autocomplete input', (done) ->
                expect(browser.html('#invitations-table tr')).not.to.be.empty()
                done()
        
        describe 'When a user clicks to resend an invitation', ->
            beforeEach (done) ->
                browser.
                    visit("#{config.site.surl}/#/room/1").#invitations").
                    then(-> browser.pressButton('input[type=submit]')).
                    then(done, done)
            
            it 'should show a message explaining the invitation has been sent', (done)->
                expect(browser.html("#server-response-container")).not.to.be.empty()
                done()


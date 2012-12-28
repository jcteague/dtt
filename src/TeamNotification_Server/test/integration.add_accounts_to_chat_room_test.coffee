expect = require('expect.js')
sinon = require('sinon')
context = require('./helpers/context')
{db: db, entities: entities, server: server, handle_in_series: handle_in_series, config: config} = require('./helpers/specs_helper')

# To not be polluting anyone's email
email_not_in_room = 'eespinal@intellisys.com.do'
users =
    name: 'users'
    entities: [
        {
            id: 1
            first_name: "'blah'"
            last_name: "'bar'"
            email: "'foo@bar.com'"
            password: "'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'"
            enabled: 1
        },
        {
            id: 2
            first_name: "'ed2'"
            last_name: "''"
            email: "'#{email_not_in_room}'"
            password: "'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'"
            enabled: 1
        },
        {
            id: 3
            first_name: "'ea'"
            last_name: "''"
            email: "'some@somewhere.com'"
            password: "'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'"
            enabled: 1
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
chat_room_users =
    name: 'chat_room_users'
    entities: [
        {
            id: 1
            user_id: 3
            chat_room_id: 1
        },
        {
            id: 2
            user_id: 1
            chat_room_id: 2
        }
    
    ] 

context.for.integration_test() (browser) ->

    describe 'Add Account To Chat Room', ->

        beforeEach (done) ->
            handle_in_series server.start(), db.save(users, chat_rooms, chat_room_users), done

        describe 'When a user visits the client#/room/:id page', ->

            beforeEach (done) ->
                browser.
                    visit("#{config.site.surl}/#/room/1").
                    then(done, done)

            it 'should contain an autocomplete input', (done) ->
                expect(browser.html('input.acInput')).to.not.be.empty()
                done()

        describe 'When a user visits the client#/room/:id page', ->

            user_id = null

            describe 'and submits in a user that exists in the system', ->

                beforeEach (done) ->
                    user_id = 2
                    browser.
                        visit("#{config.site.surl}/#/room/1"). #/users").
                        then(-> 
                            browser.fill('email', email_not_in_room)
                        ).
                        then(-> browser.pressButton('input[type=submit]')).
                        then(done, done)

                it 'should display the user added message', (done) ->
                    expect(browser.html('#server-response-container')).to.contain "User added"
                    done()

            describe 'and submits in a user that does not exist in the system', ->
                nonexistent_email = null
                beforeEach (done) ->
                    nonexistent_email = 'nonexistent@bar.com'
                    user_id = 100
                    browser.
                        visit("#{config.site.surl}/#/room/1").
                        then(-> 
                            browser.fill('email', nonexistent_email)
                        ).
                        then(-> browser.pressButton('input[type=submit]')).
                        then(done, done)

                it 'should display the user does not exist message', (done) ->
                    expect(browser.html('#server-response-container')).to.contain "An email invitation has been sent to #{nonexistent_email}"
                    done()
                    
            describe 'and there are users in room', ->
                beforeEach (done) ->
                    browser.visit("#{config.site.surl}/#/room/1").then(done, done)
                    done()
                    
                it 'should show the users in room', (done) ->
                    expect(browser.html('#room-members-container')).to.not.equal ""
                    done()
                    

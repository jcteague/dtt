expect = require('expect.js')
sinon = require('sinon')
{db: db, entities: entities, handle_in_series: handle_in_series, server: server} = require('./helpers/specs_helper')

module_loader = require('sandboxed-module')
Browser = require('zombie').Browser
room_id = 1
users =
    name: 'users'
    entities: [
        {
            id: 1
            first_name: "'foo'"
            last_name: "'foo'"
            email: "'foo@bar.com'"
            password: "'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'"
        }
        {
            id: 2
            first_name: "'bar'"
            last_name: "'bar'"
            email: "'bar@foo.com'"
            password: "'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'"
        }
    ]

chat_rooms =
    name: 'chat_room'
    entities: [
        {
            id: 1
            name: "'The real chatroom'"
            owner_id: 1
        }
    ]

generate_message = (i) ->
    return {
        "id": i
        "body": JSON.stringify({"message":"The real test"})
        "date":"2012-06-29 11:11"
        "user_id": 1
        "room_id": room_id
        "name": 'bob'
    }

describe 'Room Messages', ->

    browser = null
    browser2 = null
    describe 'Set Up', ->

        browser = null
        beforeEach (done) ->
            browser = new Browser()
            browser.authenticate().basic('foo@bar.com', '1234')
            browser2 = new Browser()
            browser2.authenticate().basic('bar@foo.com', '1234')
            db.redis.clear ["room:#{room_id}:messages"]
            handle_in_series server.start(), db.clear('users', 'chat_room','chat_room_messages'), db.create(entities.users, entities.chat_rooms,entities.chat_room_messages), db.save(users, chat_rooms), done

        describe 'When a user visits the #/room/:id/messages page', ->

            beforeEach (done) ->
                messages =
                    #name: 'chat_room_messages'
                    name: "room:#{room_id}:messages"
                    entities: (generate_message(i) for i in [1..55])

                db.redis.save(messages)
                done()
                #handle_in_series db.save(messages), done

            describe 'and wants to see the messages', ->

                beforeEach (done) ->
                    browser.visit('http://localhost:3000/#/room/1/messages').then(done, done)

                it 'should contain the messages in the container', (done) ->
                    expect(browser.html('div[id="messages-container"]')).to.not.be.empty()
                    done()

                it 'should contain not more than fifty elements at a time', (done) ->
                    expect(browser.queryAll('#messages-container p').length).to.be.lessThan(51)
                    done()

        describe 'When a user visits the #/room/:id/messages page and there are less than fifty messages', ->
            beforeEach (done) ->
                messages =
                    #name: 'chat_room_messages'
                    name: "room:#{room_id}:messages"
                    entities: (generate_message(i) for i in [1..10])

                db.redis.save(messages)
                done()
                #handle_in_series db.save(messages), done

            describe 'and wants to see the messages', ->
                beforeEach (done) ->
                    browser.visit("http://localhost:3000/#/room/1/messages").then(done, done) 
                    
                it 'should contain the messages in the container', (done) ->
                    expect(browser.html('div[id="messages-container"]')).to.not.be.empty()
                    done()
                    
                it 'should contain only ten messages if there are ten messages', (done) ->
                    expect(browser.queryAll('#messages-container p').length).to.equal(10)
                    done()

        describe 'When a user visits the #/room/:id/messages page', ->

            beforeEach (done) ->
                room_users = 
                    name: "chat_room_users"
                    entities: [ {chat_room_id:1, user_id:2}]
                handle_in_series db.save(room_users), done

            describe 'and wants to send a message', ->

                message_to_post = null
                beforeEach (done) ->
                    message_to_post = "This is indeed a pretty clever and most schoolarish of messages"
                    browser2.visit('http://localhost:3000/#/room/1/messages').then( -> )
                    browser.visit('http://localhost:3000/#/room/1/messages').
                        then( -> 
                            browser.fill("message", message_to_post)).
                        then(-> browser.pressButton('input[type=submit]')).
                        then(done, done)

                it 'should show in the message pool', (done) ->
                    expect(browser.html('#messages-container').indexOf(message_to_post)).to.not.equal(-1)
                    done()
                    
                #it 'should appear in the others users chat pages', (done) ->
                #    expect(browser2.html('#messages-container').indexOf(message_to_post)).to.not.equal(-1)
                #    done()
                    

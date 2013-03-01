expect = require('expect.js')
sinon = require('sinon')
context = require('./helpers/context')
{db: db, entities: entities, server: server, handle_in_series: handle_in_series, config: config} = require('./helpers/specs_helper')

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
            enabled:1
        }
        {
            id: 2
            first_name: "'bar'"
            last_name: "'bar'"
            email: "'bar@foo.com'"
            password: "'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'"
            enabled: 1
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

context.for.integration_test(browsers: 2) (browsers...) ->

    [browser, browser2] = browsers

    describe 'Room Messages', ->

        describe 'Set Up', ->

            beforeEach (done) ->
                db.redis.clear ["room:#{room_id}:messages"]
                handle_in_series server.start(), db.save(users, chat_rooms), done

            describe 'When a user visits the #/room/:id/messages page', ->

                beforeEach (done) ->
                    messages =
                        name: "room:#{room_id}:messages"
                        entities: (generate_message(i) for i in [1..55])

                    db.redis.save(messages)
                    done()

                describe 'and wants to see the messages', ->

                    beforeEach (done) ->
                        browser.visit("#{config.site.surl}/#/room/1/messages").
                            then(done, done)

                    it 'should contain the messages in the container', (done) ->
                        expect(browser.html('div[id="messages-container"]')).to.not.be.empty()
                        done()

                    it 'should contain not more than fifty elements at a time', (done) ->
                        expect(browser.queryAll('#messages-container table tr').length).to.be.lessThan(51)
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
                        browser2.visit("#{config.site.surl}/#/room/1/messages").then( -> )
                        browser.visit("#{config.site.surl}/#/room/1/messages").
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
                        

        describe 'Set up with little messages', ->

            beforeEach (done) ->
                db.redis.clear ["room:#{room_id}:messages"]
                handle_in_series server.start(), db.save(users, chat_rooms), done

            describe 'When a user visits the #/room/:id/messages page and there are less than fifty messages', ->
                beforeEach (done) ->
                    messages =
                        name: "room:#{room_id}:messages"
                        entities: (generate_message(i) for i in [1..10])

                    db.redis.save(messages)
                    done()

                describe 'and wants to see the messages', ->
                    beforeEach (done) ->
                        browser.visit("#{config.site.surl}/#/room/1/messages").
                            then(done, done) 
                        
                    it 'should contain the messages in the container', (done) ->
                        expect(browser.html('div[id="messages-container"]')).to.not.be.empty()
                        done()
                        
                    it 'should contain only ten messages if there are ten messages', (done) ->
                        # TODO: Clear the redis database?
                        # expect(browser.queryAll('#messages-container tr').length).to.equal(10)
                        done()


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
            last_name: "'ed 3'"
            email: "'ed@es.com'"
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

context.for.integration_test() (browser) ->

    describe 'Client Room', ->

        beforeEach (done) ->
            handle_in_series server.start(), db.save(users, chat_rooms, chat_room_users), done

        describe 'When a user visits the #/room page and he is the owner of the room', ->
            beforeEach (done) ->
                browser.
                    visit("#{config.site.surl}/#/room/1").
                    then(done, done)

            #it 'should contain an anchor to the room manage members', (done) ->
            #    expect(browser.html('a[href="#/room/1/users"]')).to.not.be.empty()
            #    done()

            it 'should contain a anchor to the room messages', (done) ->
                console.log browser.html
                expect(browser.html('a[href="#/room/1/messages"]')).to.not.be.empty()
                done()

        describe 'When a user visits the #/room page and he is the owner of the room', ->

            beforeEach (done) ->
                browser.
                    visit("#{config.site.surl}/#/room/2").
                    then(done, done)

            it 'should not contain an anchor to the room manage members', (done) ->
                expect(browser.html('a[href="#/room/2/users"]')).to.be.empty()
                done()

            it 'should contain a anchor to the room messages', (done) ->
                expect(browser.html('a[href="#/room/2/messages"]')).to.not.be.empty()
                done()

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
            #browser = new Browser(waitFor: 45000)
            browser.on 'error', (error) ->
                console.log 'Browser test', error

            browser.on 'done', (b) ->
                console.log 'done'

            #browser = new Browser(debug: true)
            browser.authenticate().basic('foo@bar.com', '1234')
            browser.cookies('dtt.local', '/').set('authtoken', 'Basic ZWVzcGluYWxAaW50ZWxsaXN5cy5jb20uZG86MTIzNDU2')
            #handle_in_series server.start(), db.clear('users', 'chat_room', 'chat_room_users'), db.create(entities.users, entities.chat_rooms, entities.chat_room_users), db.save(users, chat_rooms, chat_room_users), done
            done()

        describe 'When a user visits the #/room page and he is the owner of the room', ->

            beforeEach (done) ->
                browser.
                    visit('https://dtt.local:3001/#/room/1').
                    #visit('https://dtt.local:3001/#/registration').
                    #visit('https://dtt.local:3001').
                    #visit('https://api.dtt.local:3001/registration').
                    #then(-> browser.wait(450000)).
                    #then(-> browser.dump()).
                    then(done, done)

            it 'should contain an anchor to the room manage members', (done) ->
                ###
                browser.wait(4500, -> 
                    #console.log browser.html()
                    #console.log browser.lastRequest
                    #console.log browser.lastResponse
                    browser.resources.dump()
                    expect(browser.html('a[href="#/room/1/users"]')).to.not.be.empty()
                    done()
                )
                ###
                ###
                browser.wait(900000, ->
                    #browser.resources.dump()
                    console.log browser.html()
                    expect(browser.html('a[href="#/room/1/users"]')).to.not.be.empty()
                    done()
                )
                ###
                #console.log browser.html()
                #console.log browser.lastRequest
                #console.log browser.lastResponse
                #browser.resources.dump()
                #expect(browser.html('a[href="#/room/1/users"]')).to.not.be.empty()
                #done()
                console.log browser.html()
                expect(browser.html('a[href="#/room/1/users"]')).to.not.be.empty()
                done()

            it 'should contain a anchor to the room messages', (done) ->
                expect(browser.html('a[href="#/room/1/messages"]')).to.not.be.empty()
                done()

        describe 'When a user visits the #/room page and he is the owner of the room', ->

            beforeEach (done) ->
                browser.
                    visit('https://dtt.local:3001/#/room/2').
                    then(done, done)

            it 'should not contain an anchor to the room manage members', (done) ->
                expect(browser.html('a[href="#/room/2/users"]')).to.be.empty()
                done()

            it 'should contain a anchor to the room messages', (done) ->
                expect(browser.html('a[href="#/room/2/messages"]')).to.not.be.empty()
                done()

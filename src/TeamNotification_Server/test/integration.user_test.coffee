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

context.for.integration_test(authenticate: false) (browser) ->

    describe 'User ', ->

        beforeEach (done) ->
            handle_in_series server.start(), db.save(users, chat_rooms, chat_room_users), done

        describe 'When a user logins', ->

            describe 'if the user is invalid',  ->

                beforeEach (done) ->
                    browser.visit("#{config.site.surl}/#/user/login").then( -> 
                            browser.fill("username", "foo@bars.com")
                            browser.fill("password", "something wrong")
                        ).then(-> browser.pressButton('input[type=submit]')).
                        then(done, done)
                  
                xit 'should give the right response', (done) ->
                    expect(browser.lastResponse.body).to.equal '{}'
                    done()
        
            describe 'if the user is valid', ->

                beforeEach (done) ->
                    browser.visit("#{config.site.surl}/#/user/login").then( ->  
                            browser.fill("username", "foo@bar.com")
                            browser.fill("password", "1234")).
                        then(-> browser.pressButton('input[type=submit]')).
                        then(done, done)

                xit 'should give the right response', (done)->
                    expect(browser.lastResponse.body).to.equal JSON.stringify({"success":true,redis:config.redis, "user":{"id":1,"email":"foo@bar.com", "authtoken":"Basic Zm9vQGJhci5jb206MTIzNA=="}})
                    done()
                        
        describe 'Set Up', ->

            beforeEach (done) ->
                email = 'foo@bar.com'
                password = '1234'
                browser.authenticate().basic(email, password)
                auth_token = "Basic " + (new Buffer(email + ":" + password).toString('base64'))
                browser.cookies(config.site.host, '/').set('authtoken', auth_token)
                #browser.authenticate().basic('foo@bar.com', '1234')
                done()

            describe 'When a user visits the #/user/1 page', ->

                beforeEach (done) ->
                    browser.
                        visit("#{config.site.surl}/#/user/1").
                        then(done, done)

                it 'should contain an anchor for the user rooms and the create room', (done) ->
                    expect(browser.html('#links a[href="#/user/1/rooms"]')).to.not.be.empty()
                    expect(browser.html('#links a[href="#/room"]')).to.not.be.empty()
                    done()

                it 'should contain an anchor for each room the user is in', (done) ->
                    expect(browser.html('a[href="#/room/1"]')).to.not.be.empty()
                    expect(browser.html('a[href="#/room/2"]')).to.not.be.empty()
                    done()

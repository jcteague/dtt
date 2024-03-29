expect = require('expect.js')
sinon = require('sinon')
context = require('./helpers/context')
{db: db, entities: entities, server: server, handle_in_series: handle_in_series, config: config} = require('./helpers/specs_helper')

users =
    name: 'users'
    entities: [
        {
            id: 10
            first_name: "'blah'"
            last_name: "'bar'"
            email: "'foo@blah.com'"
            password: "'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'"
            enabled: 0
        }
    ]

context.for.integration_test(authenticate:false) (browser) ->

    describe 'User Account Registration', ->

        beforeEach (done) ->
            browser.cookies().clear()
            handle_in_series server.start(), db.save(users), done

        describe 'When a user visits the #/registration page', ->

            beforeEach (done) ->
                browser.visit("#{config.site.surl}/#/registration").then(done, done)

            it 'should contain all the inputs for the user registration', (done) ->
                console.log browser.html()
                expect(browser.html('input[name=first_name]')).to.not.be.empty()
                expect(browser.html('input[name=last_name]')).to.not.be.empty()
                expect(browser.html('input[name=email]')).to.not.be.empty()
                expect(browser.html('input[name=password]')).to.not.be.empty()
                expect(browser.html('input[name=confirm_password]')).to.not.be.empty()
                #expect(browser.html('input[type=submit]')).to.not.be.empty()
                done()

        describe 'When a user visits the #/registration page and fills the form with valid user data', ->

            beforeEach (done) ->
                browser.cookies().clear()
                browser.visit("#{config.site.surl}/#/registration").
                    then(-> 
                        browser.
                            fill('first_name', 'foo').
                            fill('last_name', 'bar').
                            fill('email', 'foo@bar.com').
                            fill('password', '123456').
                            fill('confirm_password', '123456')).
                    then(-> browser.pressButton('input[type=submit]')).
                    then(done, done)

            it 'should receive a success message', (done) ->
                expect(browser.html('#server-response-container')).to.contain "User created successfully"
                expect(browser.html('#server-response-container')).to.contain "Make sure to check your email for a confirmation link to activate your account"
                done()

        describe 'When a user visits the #/registration page and submits an email that is already registered', ->

            beforeEach (done) ->
                browser.cookies().clear()
                browser.visit("#{config.site.surl}/#/registration").
                    then(-> 
                        browser.
                            fill('first_name', 'foo').
                            fill('last_name', 'bar').
                            fill('email', 'foo@blah.com').
                            fill('password', '123456').
                            fill('confirm_password', '123456')).
                    then(-> browser.pressButton('input[type=submit]')).
                    then(done, done)

            it 'should receive a success message', (done) ->
                expect(browser.html('#server-response-container')).to.contain "Email is already registered"
                done()

        describe 'When a user visits the #/registration page and fills the form with invalid user data', ->

            beforeEach (done) ->
                browser.cookies().clear()
                browser.visit("#{config.site.surl}/#/registration").
                    then(-> 
                        browser.
                            fill('first_name', '').
                            fill('last_name', '').
                            fill('email', 'foobar.com').
                            fill('password', '123').
                            fill('confirm_password', '1234')).
                    then(-> browser.pressButton('input[type=submit]')).
                    then(done, done)

            it 'should show invalid inputs message', (done) ->
                expect(browser.queryAll('#form-container label.error').length).to.equal 5
                done()

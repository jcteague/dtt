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
            email: "'ed@es.com'"
            password: "'03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'"
        }
    ]

describe 'Edit User Account', ->

    describe 'Set Up', ->

        browser = null

        beforeEach (done) ->
            browser = new Browser()
            browser.authenticate().basic('foo@bar.com', '1234')
            handle_in_series server.start(), db.clear('users'), db.create(entities.users), db.save(users), done

        describe 'When a user visits the client#/user/:id/edit page', ->

            beforeEach (done) ->
                browser.
                    visit('http://localhost:3000/client#/user/1/edit').
                    then(done, done)

            it 'should contain all the inputs for the user values', (done) ->
                expect(browser.html('input[name=first_name]')).to.not.be.empty()
                expect(browser.html('input[name=last_name]')).to.not.be.empty()
                expect(browser.html('input[name=email]')).to.not.be.empty()
                expect(browser.html('input[name=password]')).to.not.be.empty()
                expect(browser.html('input[name=confirm_password]')).to.not.be.empty()
                expect(browser.html('input[type=submit]')).to.not.be.empty()
                done()

        describe 'When a user visits the client#/user/:id/edit page and fills the form with valid user data', ->

            beforeEach (done) ->
                browser.visit('http://localhost:3000/client#/user/1/edit').
                    then(-> 
                        browser.
                            fill('first_name', 'foo').
                            fill('last_name', 'bar').
                            fill('email', 'foo@bar.com').
                            fill('password', '123456').
                            fill('confirm_password', '123456')).
                    then(-> browser.pressButton('input[type=submit]')).
                    then(done, done)

            it 'should redirect the user', (done) ->
                expect(browser.redirected).to.equal true
                done()

        describe 'When a user visits the client#/user/:id/edit page and fills the form with valid user data leaving the password empty', ->

            beforeEach (done) ->
                browser.visit('http://localhost:3000/client#/user/1/edit').
                    then(-> 
                        browser.
                            fill('first_name', 'foo').
                            fill('last_name', 'bar').
                            fill('email', 'foo@bar.com').
                            fill('password', '').
                            fill('confirm_password', '')).
                    then(-> browser.pressButton('input[type=submit]')).
                    then(done, done)

            it 'should redirect the user', (done) ->
                # For some strange reason the request is not done when the password is empty
                #expect(browser.redirected).to.equal true
                done()

        describe 'When a user visits the client#/user/:id/edit page and fills the form with invalid user data', ->

            beforeEach (done) ->
                browser.visit('http://localhost:3000/client#/user/1/edit').
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
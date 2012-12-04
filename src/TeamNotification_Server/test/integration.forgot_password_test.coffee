expect = require('expect.js')
sinon = require('sinon')
context = require('./helpers/context')
{db: db, entities: entities, server: server, handle_in_series: handle_in_series, config: config} = require('./helpers/specs_helper')

email_not_real = 'Some@Notreal.com'
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
user_password_reset_request =
    name: 'user_password_reset_request'
    entities: [
        {
            user_id: 1
            reset_key: "'bar'"
            active:1
        },
        {
            user_id: 2,
            reset_key: "'someresetkey'"
            active:1
        }
       ]
context.for.integration_test(authenticate:false) (browser) ->
    
    describe 'Requesting password activation email', ->

        beforeEach (done) ->
            handle_in_series server.start(), db.save(users, user_password_reset_request), done

        describe 'when using an existing email', ->
            beforeEach (done) ->
                browser.
                    visit("#{config.site.surl}/#/forgot_password").
                    then(-> 
                        browser.fill('email', 'foo@bar.com')
                    ).
                    then(-> browser.pressButton('input[type=submit]')).
                        then(done, done)

            it 'should show a message telling the reset password email was sent', (done) ->
                expect(browser.html('#server-response-container p')).to.equal '<p>An email has been sent with a link to reset your password.</p>'
                done()
        
        describe 'when using an unexisting email', ->
            beforeEach (done) ->
                browser.
                    visit("#{config.site.surl}/#/forgot_password").
                    then(-> 
                        browser.fill('email', 'Some@email.com')
                    ).
                    then(-> browser.pressButton('input[type=submit]')).
                        then(done, done)
            
            it 'should show a message telling a user with that email doesnt exists', (done) ->
                expect(browser.html('#server-response-container p')).to.equal "<p>There's no user with the provided email.</p>"
                done()
        describe 'Changing the user password', ->
            describe 'when changing the password and the key is valid', ->
                beforeEach (done) ->
                    browser.
                        visit("#{config.site.surl}/#/reset_password/someresetkey").
                        then(-> 
                            browser.fill('password', '123456789')
                            browser.fill('confirm_password', '123456789')
                        ).
                        then(-> browser.pressButton('input[type=submit]')).
                            then(done, done)
                it 'should show a success message', (done) ->
                    expect(browser.html('#server-response-container p')).to.equal "<p>User password has been updated successfully</p>"
                    done()

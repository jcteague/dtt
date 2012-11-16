expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

node_hash_mock =
    sha256: sinon.stub()

email_sender_mock =
    send: sinon.spy()

email_template_mock =
    'for':
        confirmation:
            using: sinon.stub()
    
registration_service_mock =
    create_user: sinon.stub()

sut = module_loader.require('../support/factories/registration_callback_factory', {
    requires:
        'node_hash': node_hash_mock
        '../registration_service': registration_service_mock
        '../email/email_sender': email_sender_mock
        '../email/templates/email_template': email_template_mock

})

describe 'Registration Callback Factory', ->

    result = req = res = password = null

    beforeEach (done) ->
        password = 'blah password'
        req =
            body:
                first_name: ' foo'
                last_name: 'bar '
                email: 'foo@bar.com'
                password: password
        res =
            json: sinon.spy()
            flash: sinon.spy()
            
        done()

    describe 'get_for_success', ->

        user = save_promise = sanitized_user = confirmation_properties = message_template  =null

        beforeEach (done) ->
            user =
                id: 8,
                email: req.body.email
                first_name: "A clever first name"
                last_name: "A not so clever last name"
                confirmation_key: "A fake confirmation key"
                                
            confirmation_properties =
                email: user.email
                name: user.first_name+' ' + user.last_name
                confirmation_key: user.confirmation_key
          
            message_template = "some user confirmation message template"
            
            encrypted_password = 'blah encrypted password'
            node_hash_mock.sha256.withArgs(password).returns(encrypted_password)
            email_template_mock['for'].confirmation.using.withArgs(confirmation_properties).returns(message_template)
            sanitized_user =
                first_name: 'foo'
                last_name: 'bar'
                email: req.body.email
                password: encrypted_password
                enabled: 0

            user_create_promise =
                then: (cb) ->
                    cb(user)
            registration_service_mock.create_user.withArgs(sanitized_user).returns(user_create_promise)

            callback = sut.get_for_success(req, res)
            callback()
            done()

        it 'should create the user', (done) ->
            sinon.assert.calledWith(registration_service_mock.create_user, sanitized_user)
            done()
            
        it 'should send the user confirmation email', (done) ->
            sinon.assert.calledWith(email_sender_mock.send, message_template)
            done()

        it 'should send a success json response', (done) ->
            json =
                link: "/user/#{user.id}/"
                success: true
                server_messages: ['User created successfully','Make sure to check your email for a confirmation link to activate your account']
                data: 
                    id: user.id
                    email: sanitized_user.email
            sinon.assert.calledWith(res.json, json)
            done()

    describe 'get_for_failure', ->

        errors = null

        beforeEach (done) ->
            callback = sut.get_for_failure(req, res)
            errors = ['first error', 'second error']
            callback(errors)
            done()

        it 'should send a failure json response', (done) ->
            json =
                success: false
                server_messages: errors
            sinon.assert.calledWith(res.json, json)
            done()
        



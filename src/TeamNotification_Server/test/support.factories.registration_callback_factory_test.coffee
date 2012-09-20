expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

node_hash_mock =
    sha256: sinon.stub()

registration_service_mock =
    create_user: sinon.stub()

sut = module_loader.require('../support/factories/registration_callback_factory', {
    requires:
        'node_hash': node_hash_mock
        '../registration_service': registration_service_mock
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
        done()

    describe 'get_for_success', ->

        user = save_promise = sanitized_user = null

        beforeEach (done) ->
            user =
                id: 8,
                email: req.body.email
            encrypted_password = 'blah encrypted password'
            node_hash_mock.sha256.withArgs(password).returns(encrypted_password)

            sanitized_user =
                first_name: 'foo'
                last_name: 'bar'
                email: req.body.email
                password: encrypted_password

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

        it 'should send a success json response', (done) ->
            json =
                success: true
                messages: ['User created successfully']
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
                messages: errors
            sinon.assert.calledWith(res.json, json)
            done()
        



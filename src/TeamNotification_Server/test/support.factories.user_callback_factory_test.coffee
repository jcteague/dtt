expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')
_ = require('underscore')

node_hash_mock =
    sha256: sinon.stub()

repository_mock =
    update: sinon.stub()

repository_class_mock = sinon.stub()
repository_class_mock.withArgs('User').returns(repository_mock)

sut = module_loader.require('../support/factories/user_callback_factory', {
    requires:
        'node_hash': node_hash_mock
        '../repository': repository_class_mock
})

describe 'User Callback Factory', ->

    result = req = res = password = null

    beforeEach (done) ->
        password = 'blah password'
        req =
            body:
                id: 99
                first_name: ' foo'
                last_name: 'bar '
                email: 'foo@bar'
                password: password
        res =
            json: sinon.spy()
        done()

    describe 'get_for_success', ->

        user = update_promise = null

        beforeEach (done) ->
            user =
                id: req.body.id,
                email: req.body.email
            update_promise =
                then: (cb) ->
                    cb(user)

            done()

        describe 'and the password is not empty', ->

            sanitized_user = null

            beforeEach (done) ->
                encrypted_password = 'blah encrypted password'
                node_hash_mock.sha256.withArgs(password).returns(encrypted_password)

                sanitized_user =
                    id: req.body.id
                    first_name: 'foo'
                    last_name: 'bar'
                    email: req.body.email
                    password: encrypted_password
                repository_mock.update.withArgs(sanitized_user).returns(update_promise)

                callback = sut.get_for_success(req, res)
                callback()
                done()

            it 'should send a success json response', (done) ->
                json =
                    success: true
                    messages: ['User edited successfully']
                    data: 
                        id: sanitized_user.id
                        email: sanitized_user.email
                sinon.assert.calledWith(res.json, json)
                done()

        describe 'and the password is empty', ->

            sanitized_user = null

            beforeEach (done) ->
                empty_password_request = _.clone(req)
                empty_password_request.body.password = ''
                sanitized_user =
                    id: req.body.id
                    first_name: 'foo'
                    last_name: 'bar'
                    email: req.body.email
                repository_mock.update.withArgs(sanitized_user).returns(update_promise)

                callback = sut.get_for_success(empty_password_request, res)
                callback()
                done()

            it 'should send a success json response', (done) ->
                json =
                    success: true
                    messages: ['User edited successfully']
                    data: 
                        id: sanitized_user.id
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
        



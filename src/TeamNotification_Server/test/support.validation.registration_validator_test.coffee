expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')
_ = require('underscore')
Q = require('q')

repository_class_mock = sinon.stub()
user_repository_mock =
    find: sinon.stub()

repository_class_mock.withArgs('User').returns(user_repository_mock)
sut = module_loader.require('../support/validation/registration_validator', {
    requires:
        '../repository': repository_class_mock
})

describe 'Registration Validator', ->

    describe 'validate', ->

        user_data = success_callback = failure_callback = null

        make_promise_for = (return_value) ->
            Q.fcall () -> return_value

        beforeEach (done) ->
            success_callback = sinon.mock()
            failure_callback = sinon.mock()
            user_data =
                first_name: 'foo'
                last_name: 'bar'
                email: 'foo@bar.com'
                password: '123456'

            done()

        describe 'and the user data is valid', ->

            describe 'and the email is not already registered', ->

                beforeEach (done) ->
                    users_promise = make_promise_for null
                    user_repository_mock.find.withArgs(email: user_data.email).returns(users_promise)
                    handler = sut.validate(user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the success callback', (done) ->
                    success_callback.exactly(1)
                    done()

            describe 'and the email is already registered', ->

                beforeEach (done) ->
                    users_promise = make_promise_for ['foo user']
                    user_repository_mock.find.withArgs(email: user_data.email).returns(users_promise)
                    handler = sut.validate(user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback', (done) ->
                    failure_callback.exactly(1).withArgs(['Email is already registered'])
                    done()

        describe 'and the user data is not valid', ->

            errors = null

            describe 'and the first name is empty', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.first_name = ''

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the first name is invalid error', (done) ->
                    failure_callback.exactly(1).withArgs(['First name is invalid'])
                    done()

            describe 'and the first name contains invalid characters', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.first_name = 'foo 2'

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the first name is invalid error', (done) ->
                    failure_callback.exactly(1).withArgs(['First name is invalid'])
                    done()

            describe 'and the last name is empty', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.last_name = ''

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the last name is invalid error', (done) ->
                    failure_callback.exactly(1).withArgs(['Last name is invalid'])
                    done()

            describe 'and the last name contains invalid characters', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.last_name = 'bar 2'

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the last name is invalid error', (done) ->
                    failure_callback.exactly(1).withArgs(['Last name is invalid'])
                    done()

            describe 'and the email is empty', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.email = ''

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the email is invalid error', (done) ->
                    failure_callback.exactly(1).withArgs(['Email is invalid'])
                    done()

            describe 'and the email contains is invalid', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.email = 'fsa'

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the email is invalid error', (done) ->
                    failure_callback.exactly(1).withArgs(['Email is invalid'])
                    done()

            describe 'and the password is too short', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.password = 'fsa'

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the password must contain at least 6 characters', (done) ->
                    failure_callback.exactly(1).withArgs(['Password must contain at least 6 characters'])
                    done()

            describe 'and the password contains invalid characters', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.password = 'fsa fod'

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the password contains invalid characters error', (done) ->
                    failure_callback.exactly(1).withArgs(['Password contains invalid characters'])
                    done()


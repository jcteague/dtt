expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')
_ = require('underscore')

sut = module_loader.require('../support/validation/user_validator', {})

describe 'User Validator', ->

    describe 'validate', ->

        user_data = success_callback = failure_callback = null

        beforeEach (done) ->
            success_callback = sinon.spy()
            failure_callback = sinon.spy()
            user_data =
                id: 9
                first_name: 'foo'
                last_name: 'bar'
                email: 'foo@bar.com'
                password: '123456'

            done()

        describe 'and the user data is valid', ->

            describe 'and the password is filled', ->

                beforeEach (done) ->
                    handler = sut.validate(user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the success callback', (done) ->
                    sinon.assert.called(success_callback)
                    done()

            describe 'and the password is empty', ->

                beforeEach (done) ->
                    user_data_with_empty_password = _.clone(user_data)
                    user_data_with_empty_password.password = ''
                    handler = sut.validate(user_data_with_empty_password)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the success callback', (done) ->
                    sinon.assert.called(success_callback)
                    done()

        describe 'and the user data is not valid', ->

            errors = null

            describe 'and the id is empty', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.id = ''

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the id must be provided error', (done) ->
                    sinon.assert.calledWith(failure_callback, ['Id must be provided'])
                    done()

            describe 'and the first name is empty', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.first_name = ''

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the first name is invalid error', (done) ->
                    sinon.assert.calledWith(failure_callback, ['First name is invalid'])
                    done()

            describe 'and the first name contains invalid characters', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.first_name = 'foo 2'

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the first name is invalid error', (done) ->
                    sinon.assert.calledWith(failure_callback, ['First name is invalid'])
                    done()

            describe 'and the last name is empty', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.last_name = ''

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the last name is invalid error', (done) ->
                    sinon.assert.calledWith(failure_callback, ['Last name is invalid'])
                    done()

            describe 'and the last name contains invalid characters', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.last_name = 'bar 2'

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the last name is invalid error', (done) ->
                    sinon.assert.calledWith(failure_callback, ['Last name is invalid'])
                    done()

            describe 'and the email is empty', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.email = ''

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the email is invalid error', (done) ->
                    sinon.assert.calledWith(failure_callback, ['Email is invalid'])
                    done()

            describe 'and the email contains is invalid', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.email = 'fsa'

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the email is invalid error', (done) ->
                    sinon.assert.calledWith(failure_callback, ['Email is invalid'])
                    done()

            describe 'and the password is too short', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.password = 'fsa'

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the password must contain at least 6 characters', (done) ->
                    sinon.assert.calledWith(failure_callback, ['Password must contain at least 6 characters'])
                    done()

            describe 'and the password contains invalid characters', ->

                beforeEach (done) ->
                    invalid_user_data = _.clone(user_data)
                    invalid_user_data.password = 'fsa fod'

                    handler = sut.validate(invalid_user_data)
                    handler.handle_with success_callback, failure_callback
                    done()

                it 'should call the failure callback with the password contains invalid characters error', (done) ->
                    sinon.assert.calledWith(failure_callback, ['Password contains invalid characters'])
                    done()

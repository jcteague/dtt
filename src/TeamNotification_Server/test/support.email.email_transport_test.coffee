expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

q_mock =
    defer: sinon.stub()

email_transport_factory_mock =
    get: sinon.stub()
transport = sendMail: (options, callback) ->

email_transport_factory_mock.get.returns(transport)

sut = module_loader.require('../support/email/email_transport', {
    requires:
        'q': q_mock
        './email_transport_factory': email_transport_factory_mock
})

describe 'Email Transport', ->

    describe 'send_email', ->

        result = expected_result = deferred = null

        beforeEach (done) ->
            deferred =
                promise: 'q-promise'
                resolve: sinon.stub()
                reject: sinon.stub()

            q_mock.defer.returns deferred
            expected_result = deferred.promise
            done()

        describe 'and the email is sent without errors', ->

            response = null

            beforeEach (done) ->
                email_message = 
                    to: 'foo@bar.com', 
                    html: 'email message'
                response = 'good response'
                transport.sendMail = (options, callback) ->
                    callback(false, response)
                result = sut.send_email email_message
                done()

            it 'should return a promise', (done) ->
                expect(result).to.equal expected_result
                done()

            it 'should resolve the promise with transport response', (done) ->
                sinon.assert.calledWith(deferred.resolve, response)
                done()

        describe 'and the email has an email address that is not a valid email address', ->

            beforeEach (done) ->
                email_message = 
                    to: 'foo.com', 
                    html: 'email message'
                result = sut.send_email email_message
                done()

            it 'should return a promise', (done) ->
                expect(result).to.equal expected_result
                done()

            it 'should reject the promise with a malformed email address error', (done) ->
                sinon.assert.calledWith(deferred.reject, 'Malformed email addresses')
                done()

        describe 'and the email has a list of email addresses and one is invalid', ->

            email_send_spy = response = null

            beforeEach (done) ->
                email_message = 
                    to: 'foo@bar.com, foo.com, blah@foo.org', 
                    html: 'email message'

                email_send_spy = sinon.spy()
                response = 'good response'
                transport.sendMail = (options, callback) ->
                    email_send_spy(options.to)
                    callback(false, response)
                result = sut.send_email email_message
                done()

            it 'should return a promise', (done) ->
                expect(result).to.equal expected_result
                done()

            it 'should send the email to the valid email address', (done) ->
                matcher = (value) ->
                    value == 'foo@bar.com,blah@foo.org'
                sinon.assert.calledWith(email_send_spy, sinon.match(matcher))
                done()

            it 'should resolve the promise with transport response', (done) ->
                sinon.assert.calledWith(deferred.resolve, response)
                done()


        describe 'and the email cannot be sent due to errors', ->

            error = null

            beforeEach (done) ->
                email_message = 
                    to: 'foo@bar.com', 
                    html: 'email message'
                error = 'blah error'
                transport.sendMail = (options, callback) ->
                    callback(error)
                result = sut.send_email email_message
                done()

            it 'should return a promise', (done) ->
                expect(result).to.equal expected_result
                done()

            it 'should reject the promise with the error', (done) ->
                sinon.assert.calledWith(deferred.reject, error)
                done()

expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

q_mock =
    defer: sinon.stub()

nodemailer_mock =
    createTransport: sinon.stub()

gmail_smtp_options_mock = 'gmail smtp options'
transport = sendMail: (options, callback) ->

nodemailer_mock.createTransport.withArgs('SMTP', gmail_smtp_options_mock).returns(transport)

sut = module_loader.require('../support/email/email_transport', {
    requires:
        'nodemailer': nodemailer_mock
        'q': q_mock
        './gmail_smtp_options': gmail_smtp_options_mock
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
                email_message = 'email message'
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

        describe 'and the email cannot be sent due to errors', ->

            error = null

            beforeEach (done) ->
                email_message = 'email message'
                error = 'blah error'
                transport.sendMail = (options, callback) ->
                    callback(error)
                nodemailer_mock.createTransport.withArgs('SMTP', gmail_smtp_options_mock).returns(transport)
                result = sut.send_email email_message
                done()

            it 'should return a promise', (done) ->
                expect(result).to.equal expected_result
                done()

            it 'should reject the promise with the error', (done) ->
                sinon.assert.calledWith(deferred.reject, error)
                done()

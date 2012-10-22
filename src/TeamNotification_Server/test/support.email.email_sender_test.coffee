expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

email_transport_mock =
    send_email: sinon.stub()

sut = module_loader.require('../support/email/email_sender', {
    requires:
        './email_transport': email_transport_mock
})

describe 'Email Sender', ->

    describe 'send', ->

        result = email_message = send_email_promise = null

        beforeEach (done) ->
            email_message = 'blah email message'
            send_email_promise = 'blah promise'
            email_transport_mock.send_email.withArgs(email_message).returns send_email_promise
            result = sut.send email_message
            done()

        it 'should send the email through the transport', (done) ->
            sinon.assert.calledWith(email_transport_mock.send_email, email_message)
            done()

        it 'should return the promise returned from the transport', (done) ->
            expect(result).to.equal send_email_promise
            done()

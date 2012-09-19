expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

nodemailer_mock =
    createTransport: sinon.stub()

gmail_smtp_options_mock = 'gmail smtp options'

valid_transports_matcher = (value) ->
    value in ['SMTP', 'SES']

sut = module_loader.require('../support/email/email_transport_factory', {
    requires:
        'nodemailer': nodemailer_mock
        './gmail_smtp_options': gmail_smtp_options_mock
})

describe 'Email Transport Factory', ->

    describe 'get', ->

        result = transport = null

        beforeEach (done) ->
            transport = 'blah transport'
            nodemailer_mock.createTransport.withArgs(sinon.match(valid_transports_matcher), gmail_smtp_options_mock).returns(transport)
            result = sut.get()
            done()

        it 'should return the email transport instance', (done) ->
            expect(result).to.equal transport
            done()

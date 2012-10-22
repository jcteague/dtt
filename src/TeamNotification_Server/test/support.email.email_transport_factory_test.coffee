expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

nodemailer_mock =
    createTransport: sinon.stub()

email_configuration = 'smtp options'

valid_transports_matcher = (value) ->
    value in ['SMTP', 'SES']

transport_factory_mock =
    get: sinon.stub()

sut = module_loader.require('../support/email/email_transport_factory', {
    requires:
        'nodemailer': nodemailer_mock
        './email_configuration': email_configuration
        '../../test/helpers/mock_email_transport': transport_factory_mock
})

describe 'Email Transport Factory', ->

    describe 'get', ->

        result = transport = null

        beforeEach (done) ->
            transport = 'blah transport'
            nodemailer_mock.createTransport.withArgs(sinon.match(valid_transports_matcher), email_configuration).returns(transport)
            transport_factory_mock.get.returns(transport)
            result = sut.get()
            done()

        it 'should return the email transport instance', (done) ->
            expect(result).to.equal transport
            done()

expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

support_mock =
    core:
        entity_factory:
            get: sinon.stub()

express_mock =
    bodyParser: sinon.stub()

routes_service_mock =
    build: sinon.stub()
    get_messages_from_flash: sinon.stub()
    get_server_response: sinon.stub()
 
user_validator_mock =
    validate: sinon.stub()

user_callback_factory_mock =
    get_for_success: sinon.stub()
    get_for_failure: sinon.stub()

node_hash_mock =
    sha256: sinon.stub()

config_mock = () ->
    site:
        surl: 'someurl'
repository_mock = sinon.stub()

email_sender_mock =
    send: sinon.stub()
email_template_mock =
    'for':
        password_reset:
            using: sinon.stub()

password_request = module_loader.require('../subdomains/default/routes/user', {
    requires:
        'node_hash': node_hash_mock
        '../../../config': config_mock
        '../../../support/repository': repository_mock
        '../../../support/routes_service': routes_service_mock
        'express': express_mock
        '../../../support/email/email_sender': email_sender_mock
        '../../../support/email/templates/email_template': email_template_mock
})

describe 'Reset password', ->

    describe 'build_routes', ->
        app = null
        body_parser_result = null
        beforeEach (done) ->
            app = { get:sinon.spy(), post:sinon.spy() }
            body_parser_result = 'blah'
            express_mock.bodyParser.returns(body_parser_result)
            password_request.build_routes(app)
            done()

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith app.get, '/api/forgot_password', password_request.methods.forgot_password_form
            sinon.assert.calledWith app.post,'/api/forgot_password', body_parser_result, password_request.methods.send_reset_email
            sinon.assert.calledWith app.get, '/api/reset_password/:reset_key', password_request.methods.reset_form
            sinon.assert.calledWith app.post, '/api/reset_password/:reset_key', body_parser_result, methods.reset_password
            done()

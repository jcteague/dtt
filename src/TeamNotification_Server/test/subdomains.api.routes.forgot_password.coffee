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
user_validator_mock =
    validate: sinon.stub()

user_callback_factory_mock =
    get_for_success: sinon.stub()
    get_for_failure: sinon.stub()

user = module_loader.require('../subdomains/default/routes/user', {
    requires:
        '../../../support/routes_service': routes_service_mock
        'express': express_mock
})

describe 'Reset password', ->

    describe 'build_routes', ->
        app = null
        body_parser_result = null
        beforeEach (done) ->
            app = { get:sinon.spy(), post:sinon.spy() }
            body_parser_result = 'blah'
            express_mock.bodyParser.returns(body_parser_result)
            user.build_routes(app)
            done()

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get,'/api/users/query',user.methods.get_users)
            sinon.assert.calledWith(app.get,'/api/user/login',user.methods.login)
            sinon.assert.calledWith(app.post,'/api/user/login',body_parser_result,user.methods.authenticate)
            sinon.assert.calledWith(app.get,'/api/user/:id',user.methods.get_user)
            sinon.assert.calledWith(app.get,'/api/user/:id/edit',user.methods.get_user_edit)
            sinon.assert.calledWith(app.post,'/api/user/:id/edit',user.methods.post_user_edit)
            sinon.assert.calledWith(app.get,'/api/user/:id/rooms',user.methods.get_user_rooms)
            sinon.assert.calledWith(app.get,'/api/users',user.methods.redir_user)
            done()

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
    add_user_data_to_collection: sinon.stub()
    
user_validator_mock =
    validate: sinon.stub()

user_callback_factory_mock =
    get_for_success: sinon.stub()
    get_for_failure: sinon.stub()
    
socket_manager_mock = 
    set_socket_events: sinon.stub()
    
redis_mock =
    open: sinon.stub()

client_mock =
    auth: sinon.stub()
    publish: sinon.stub()
    subscribe: sinon.stub()
    on: sinon.stub()
    zadd: sinon.stub()

redis_mock.open.returns(client_mock)

middleware_mock =
    socket_io: sinon.stub()
    valid_user: sinon.stub()


user = module_loader.require('../subdomains/default/routes/user', {
    requires:
        '../../../support/validation/user_validator': user_validator_mock
        '../../../support/factories/user_callback_factory': user_callback_factory_mock
        '../../../support/routes_service': routes_service_mock
        '../../../support/socket/socket_manager': socket_manager_mock
        '../../../support/middlewares': middleware_mock
        '../../../support/redis/redis_gateway': redis_mock
        'express': express_mock
})

describe 'User', ->

    describe 'build_routes', ->
        app = null
        body_parser_result = null
        socket_middleware_result = null
        valid_user_result = null
        beforeEach (done) ->
            app = { get:sinon.spy(), post:sinon.spy() }
            io = sinon.stub()
            body_parser_result = 'blah'
            express_mock.bodyParser.returns(body_parser_result)
            socket_middleware_result = 'Awesome!'
            valid_user_result = 'valid user mock'
            middleware_mock.socket_io.withArgs(io).returns(socket_middleware_result)
            middleware_mock.valid_user.returns(valid_user_result)
            user.build_routes(app, io)
            done()

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get,'/api/users/query',user.methods.get_users)
            sinon.assert.calledWith(app.get,'/api/user/login',user.methods.login)
            sinon.assert.calledWith(app.post,'/api/user/login',body_parser_result, user.methods.authenticate)
            sinon.assert.calledWith(app.get,'/api/user/:id', socket_middleware_result, valid_user_result, user.methods.get_user)
            sinon.assert.calledWith(app.get,'/api/user/:id/edit',valid_user_result, user.methods.get_user_edit)
            sinon.assert.calledWith(app.post,'/api/user/:id/edit',valid_user_result, user.methods.post_user_edit)
            sinon.assert.calledWith(app.get,'/api/user/:id/rooms',valid_user_result, user.methods.get_user_rooms)
            sinon.assert.calledWith(app.get,'/api/users',user.methods.redir_user)
            done()

    describe 'methods', ->

        collection = collection_value = collection_factory = res = req = user_id = null

        beforeEach (done) ->
            collection_value = 'blah collection'
            collection =
                to_json: ->
                    collection_value
            add_user_data_to_collection_callback =
                then: (callback)->
                    callback(collection_value)
                
            collection_factory =
                for: sinon.stub()
            req =
                param: sinon.stub()
                socket_io: 
                    of: sinon.stub()
            user_id = 10
            req.param.withArgs('id').returns(user_id)
            
            routes_service_mock.add_user_data_to_collection.returns(add_user_data_to_collection_callback)
            res = 
                json: sinon.spy()
            done()
        
        describe 'get_user', ->

            beforeEach (done) ->
                routes_service_mock.build.withArgs('user_collection').returns(collection_factory)
                user_collection =
                    fetch_to: (callback) ->
                        callback(collection)
                collection_factory.for.withArgs(user_id).returns(user_collection)
                user.methods.get_user(req, res)
                done()

            it 'should return the built collection for the user model', (done) ->
                sinon.assert.calledWith(res.json, collection_value)
                done()

        describe 'get_user_edit', ->

            beforeEach (done) ->
                routes_service_mock.build.withArgs('user_edit_collection').returns(collection_factory)
                user_edit_collection =
                    fetch_to: (callback) ->
                        callback(collection)
                collection_factory.for.withArgs(user_id).returns(user_edit_collection)
                user.methods.get_user_edit(req, res)
                done()

            it 'should return the built collection for the edit user', (done) ->
                sinon.assert.calledWith(res.json, collection_value)
                done()

        describe 'post_user_edit', ->

            validation_result = success_callback = failure_callback = null

            beforeEach (done) ->
                req.body = 
                    first_name: 'foo'
                    last_name: 'bar'
                    email: 'foo@bar.com'
                    password: '1234'
                validation_result =
                    handle_with: sinon.stub()
                user_validator_mock.validate.withArgs(req.body).returns validation_result

                success_callback = 'success callback'
                user_callback_factory_mock.get_for_success.withArgs(req, res).returns(success_callback)

                failure_callback = 'failure callback'
                user_callback_factory_mock.get_for_failure.withArgs(req, res).returns(failure_callback)

                user.methods.post_user_edit(req, res)
                done()

            it 'should call the validation result with the edit success and edit failure callbacks', (done) ->
                sinon.assert.calledWith(validation_result.handle_with, success_callback, failure_callback)
                done()

        describe 'get_user_rooms', ->

            beforeEach (done) ->
                routes_service_mock.build.withArgs('user_rooms_collection').returns(collection_factory)
                user_rooms_collection =
                    fetch_to: (callback) ->
                        callback(collection)
                collection_factory.for.withArgs(user_id).returns(user_rooms_collection)
                user.methods.get_user_rooms(req, res)
                done()

            it 'should return the built collection for the user rooms model', (done) ->
                sinon.assert.calledWith(res.json, collection_value)
                done()

        describe 'get_users', ->

            username = null

            beforeEach (done) ->
                routes_service_mock.build.withArgs('users_collection').returns(collection_factory)
                username = 'bla'
                req.param.withArgs('q').returns(username)
                user_rooms_collection =
                    fetch_to: (callback) ->
                        callback(collection)
                collection_factory.for.withArgs(username).returns(user_rooms_collection)
                user.methods.get_users(req, res)
                done()

            it 'should return the built collection for the user rooms model', (done) ->
                sinon.assert.calledWith(res.json, collection_value)
                done()

        describe 'login', ->
            json_data = null
            template = null
            links = null
            href = null
            beforeEach (done) ->
                req = 
                    flash: ()->
                        return ""
                res = 
                    json: (json) ->
                        json_data = json
                routes_service_mock.get_messages_from_flash.withArgs("").returns([])
                
                user.methods.login(req, res)
                template = json_data['template']
                links = json_data['links']
                href = json_data['href']
                done()
            
            it 'should contain a link to itself', (done) -> 
                expect(links[0]).to.eql {"name": "self", "rel": "login", 'href':'/user/login'}
                done()
                
            it 'should contain a link to the forgot password option', (done) -> 
                expect(links[1]).to.eql {"name": "forgot password", "rel": "forgot_password", 'href':'/forgot_password'}
                done()
                
            it 'should contain the login form', (done) ->
                expect(template.data[0]).to.eql {'name':'username', 'label':'Email', 'type':'string'}
                expect(template.data[1]).to.eql {'name':'login_password', 'label':'Password', 'type':'password'}
                done()


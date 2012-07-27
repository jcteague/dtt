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

user = module_loader.require('../routes/user.js', {
    requires:
        '../support/routes_service': routes_service_mock
        'express': express_mock
})

describe 'User', ->

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
            sinon.assert.calledWith(app.get,'/users/query',user.methods.get_users)
            sinon.assert.calledWith(app.get,'/user/login',user.methods.login)
            sinon.assert.calledWith(app.post,'/user/login',body_parser_result,user.methods.authenticate)
            sinon.assert.calledWith(app.get,'/user/:id',user.methods.get_user)
            sinon.assert.calledWith(app.get,'/user/:id/rooms',user.methods.get_user_rooms)
            sinon.assert.calledWith(app.get,'/users',user.methods.redir_user)
            done()

    describe 'methods', ->

        collection = null
        collection_value = null
        collection_factory = null
        res = null
        req = null
        user_id = null

        beforeEach (done) ->
            collection_value = 'blah collection'
            collection =
                to_json: ->
                    collection_value

            collection_factory =
                for: sinon.stub()
            req =
                param: sinon.stub()
            user_id = 10
            req.param.withArgs('id').returns(user_id)
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
                req = null
                res = 
                    json: (json) ->
                        json_data = json
                user.methods.login(req, res)
                template = json_data['template']
                links = json_data['links']
                href = json_data['href']
                done()
            
            it 'should contain a link to itself', (done) -> 
                expect(links[0]).to.eql {"name": "self", "rel": "login", 'href':'/user/login'}
                done()
                
            it 'should contain the login form', (done) ->
                expect(template.data[0]).to.eql {'name':'username', 'label':'Username', 'type':'string'}
                expect(template.data[1]).to.eql {'name':'password', 'label':'Password', 'type':'password'}
                done()


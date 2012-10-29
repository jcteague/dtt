expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

Q = require('q')

express_mock =
    bodyParser: sinon.stub()
    
http_mock =
    request: sinon.stub()

querystring_mock =
    stringify: sinon.stub()

support_mock = 
    core:
        entity_factory: 
            create: sinon.stub()

repository_class_mock = sinon.stub()

config = require('../config')()

routes_service_mock =
    build: sinon.stub()
    add_user_to_chat_room: sinon.stub()
    is_user_in_room: sinon.stub()
    get_server_response: sinon.stub()

github_helper_mock =
    set_github_repository_events: sinon.stub()
    get_event_message_object: sinon.stub()

redis_mock =
    open: sinon.stub()

client_mock =
    auth: sinon.stub()
    publish: sinon.spy()
    subscribe: sinon.stub()
    on: sinon.stub()
    zadd: sinon.spy()

redis_mock.open.returns(client_mock)

chat_room_mock =
    find: sinon.stub()

chat_room_message_mock =
    save: sinon.spy()
    
sut = module_loader.require('../subdomains/api/routes/github', {
    requires:
        'express': express_mock
        'https': http_mock
        'querystring': querystring_mock
        '../../../support/core': support_mock
        '../../../support/repository': repository_class_mock
        '../../../support/routes_service': routes_service_mock
        '../../../support/redis/redis_gateway': redis_mock
        '../../../support/github_events_helper': github_helper_mock
})

push_object =
    pusher:
        name: 'none',
    repository: 
        name: 'oauth2orize',
        url: 'https://github.com/eatskolnikov/oauth2orize',
        language: 'JavaScript',
        id: 5677704
        owner:
            name: 'eatskolnikov'
            email: 'eats007@gmail.com'

describe 'Github', ->

    describe 'build_routes', ->
        app = null
        io = null
        body_parser_result = null
        socket_middleware_result = null
        beforeEach (done) ->
            collection_value = 'blah collection value'
            collection =
                to_json: ->
                    collection_value
            app =
                get:sinon.spy()
                post:sinon.spy()
            body_parser_result = 'blah'
            express_mock.bodyParser.returns(body_parser_result)
            sut.build_routes(app, io)
            done()

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get,'/github/repositories/:access_token', sut.methods.github_repositories)
            sinon.assert.calledWith(app.post,'/github/repositories/:access_token', body_parser_result, sut.methods.associate_github_repositories)
            done()

    describe 'associate_github_repositories', ->
        req = res = random_token = null
        beforeEach (done) ->
            repos = ["First"]
            owner = "someone"
            room_key = "random room key"
            random_token = "random access token"
            res =
                json: sinon.spy()
            req =
                body:
                    repositories: repos
                    owner: owner
                    room_key: room_key
                param:sinon.stub()
            
            req.param.withArgs('access_token').returns random_token
            
            github_helper_mock.set_github_repository_events.withArgs(repos,owner,room_key,random_token).returns "result"
            routes_service_mock.get_server_response.withArgs(true, ["The webhooks were successfully created"]).returns "server response"
            
            sut.methods.associate_github_repositories(req,res) 
            done()

        it "should create the webhook", (done) ->
            expect(res.json.calledWith("server response")).to.equal(true)
            done()


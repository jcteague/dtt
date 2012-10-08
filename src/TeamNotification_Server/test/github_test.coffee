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
    publish: sinon.stub()
    subscribe: sinon.stub()
    on: sinon.stub()
    zadd: sinon.stub()

redis_mock.open.returns(client_mock)

chat_room_mock =
    find: sinon.stub()

sut = module_loader.require('../routes/github.js', {
    requires:
        'express': express_mock
        'https': http_mock
        'querystring': querystring_mock
        '../support/core': support_mock
        '../support/repository': repository_class_mock
        '../support/routes_service': routes_service_mock
        '../support/redis/redis_gateway': redis_mock
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

    describe 'Receiving a github event', ->
        req = res = null
        beforeEach (done) ->
            repository_class_mock.withArgs('ChatRoom').returns chat_room_mock
            done()

        describe 'and the event is a push', ->

            beforeEach (done) ->
                req =
                    param: sinon.stub()
                    body: push_object

                done()



































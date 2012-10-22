expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

Q = require('q')
config = require('../config')()
promised_http_requester_mock =
    request: sinon.stub()

event_object_mapper_mock =
    map: sinon.stub()

sut = module_loader.require('../support/github_events_helper', {
    requires:
        './http/promised_http_requester': promised_http_requester_mock
        './external_services/github/messages_mapper': event_object_mapper_mock
})

events = [ 'push', 'issues', 'issue_comment', 'commit_comment', 'pull_request', 'fork']

describe 'Github Events Helper', ->

    describe 'set_github_repository_events', ->

        result = null

        beforeEach (done) ->
            github_repositories = ['repo1', 'repo2', 'repo3']
            owner = 'owner'
            room_key = 'key'
            access_token = 'token'

            post_data =
                name : "web"
                events: events
                config:
                    content_type: "json"
                    url:"#{config.site.url}/github/events/#{room_key}"

            promised_http_requester_mock.request.withArgs(JSON.stringify(post_data), sinon.match.object).returns(Q.resolve('promise'))

            result = sut.set_github_repository_events(github_repositories, owner, room_key, access_token)
            done()

        afterEach (done) ->
            promised_http_requester_mock.request.reset()
            done()

        it 'should return the joined promises', (done) ->
            result.then (promises) ->
                expect(promises).to.eql ['promise', 'promise', 'promise']
                done()


    describe 'get_event_message_object', ->

        result = expected_result = null

        beforeEach (done) ->
            event_obj = 'blah object'
            mapped_obj = 'blah mapped object'
            event_object_mapper_mock.map.withArgs(event_obj).returns mapped_obj
            expected_result = mapped_obj
            result = sut.get_event_message_object event_obj
            done()

        it 'should return the mapped event object', (done) ->
            expect(result).to.eql expected_result
            done()




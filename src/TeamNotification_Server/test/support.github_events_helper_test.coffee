expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

config = require('../config')()
https_mock =
    request: sinon.stub()

sut = module_loader.require('../support/github_events_helper', {
    requires:
        'https' : https_mock
})


describe 'Github Events Helper', ->

    beforeEach (done) ->
        done()

    describe 'set_github_repository_events', ->

        result = expected_result = null

        beforeEach (done) ->
            result = sut.set_github_repository_events
            done()

        xit 'should make a request for each repository', (done) ->
            for repository in github_repositories
                sinon.assert.calledWith(post_request)
            done()

        xit 'should return a promise', (done) ->
            expect(result).to.eql expected_result
            done()


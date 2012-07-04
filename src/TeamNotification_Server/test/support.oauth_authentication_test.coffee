expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

passport_mock = 
    use: sinon.stub()
    initialize: sinon.stub()
    authenticate: sinon.stub()

underscore_mock =
    bindAll: sinon.stub()

OAuthAuthentication = module_loader.require('../support/oauth_authentication', {
    requires:
        'passport': passport_mock
        'underscore': underscore_mock
})

describe 'OAuth Authentication', ->

    sut = oauth2_strategy = null

    beforeEach (done) ->
        oauth2_strategy = sinon.stub()
        sut = new OAuthAuthentication(oauth2_strategy)
        done()

    it 'should tell passport to use a new OAuth2 Strategy', (done) ->
        sinon.assert.calledWith(passport_mock.use, oauth2_strategy)
        done()

    describe 'get_strategy', ->

        result = expected_result = null

        beforeEach (done) ->
            result = sut.get_strategy()
            done()

        it 'should return an instance of an OAuth2Strategy', (done) ->
            expect(result.constructor.name).to.equal 'OAuth2Strategy'
            done()

    describe 'initializeAuth', ->

        beforeEach (done) ->
            sut.initializeAuth()
            done()

        it 'should inialize passport', (done) ->
            sinon.assert.called(passport_mock.initialize)
            done()

    describe 'authenticate', ->

        beforeEach (done) ->
            sut.authenticate(null, null)
            done()

        it 'should authenticate with basic', (done) ->
            sinon.assert.calledWith(passport_mock.authenticate, 'oauth2', {})
            done()



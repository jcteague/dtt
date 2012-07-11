expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

passport_mock = 
    use: sinon.stub()
    initialize: sinon.stub()
    authenticate: sinon.stub()

underscore_mock =
    bindAll: sinon.stub()

hash_mock = 
    sha256: sinon.stub()

config_mock = ->
    return {
        site:
            whitelisted_paths: ['blah path']
    }

Authentication = module_loader.require('../support/authentication', {
    requires:
        'passport': passport_mock
        'underscore': underscore_mock
        'node_hash': hash_mock
        '../config': config_mock
})

describe 'Authentication', ->

    sut = basic_strategy = repository = null

    beforeEach (done) ->
        basic_strategy = sinon.stub()
        sut = new Authentication(basic_strategy)
        done()

    it 'should tell passport to use the basic strategy', ->
        sinon.assert.calledWith(passport_mock.use, sut.basic_strategy)

    it 'should have instantiated the BasicStrategy', (done) ->
        expect(sut.basic_strategy).to.equal basic_strategy
        done()

    it 'should have instantiated the repository', (done) ->
        expect(sut.repository.constructor.name).to.equal 'Repository'
        done()

    it 'should bind the findByUserName to the Authentication object', (done) ->
        sinon.assert.calledWith(underscore_mock.bindAll, sut)
        done()

    describe 'initializeAuth', ->

        beforeEach (done) ->
            sut.initializeAuth()
            done()

        it 'should inialize passport', (done) ->
            sinon.assert.called(passport_mock.initialize)
            done()

    describe 'findByUserName', ->

        username = password = encrypted = user = null

        beforeEach (done) ->
            username = 'blah user'
            password = 'foo password'
            encrypted = 'bar encrypted password'
            done()

        describe 'and the user exists on the database', ->

            express_done = null

            beforeEach (done) ->
                hash_mock.sha256.withArgs(password).returns(encrypted)
                user = {id: 2, email: username, password: encrypted}
                promise = 
                    then: (callback) ->
                        callback([user])
                sinon.stub(sut.repository, 'find').withArgs(email: username).returns(promise)
                done()

            describe 'and the password matches the username', ->

                beforeEach (done) ->
                    express_done = sinon.spy()
                    sut.findByUserName(username, password, express_done)
                    done()

                it 'should call the done with the user data', (done) ->
                    sinon.assert.calledWith(express_done, null, {id: user.id, email: user.email})
                    done()

            describe 'and the password does not match the username', ->

                beforeEach (done) ->
                    express_done = sinon.spy()
                    sut.findByUserName(username, 'foo123f', express_done)
                    done()

                it 'should call the done with false', (done) ->
                    sinon.assert.calledWith(express_done, null, false)
                    done()

        describe 'and the user does not exist in the database', ->

            express_done = null

            beforeEach (done) ->
                express_done = sinon.spy()
                promise = 
                    then: (callback) ->
                        callback(null)
                sinon.stub(sut.repository, 'find').withArgs(email: username).returns(promise)

                sut.findByUserName(username, password, express_done)
                done()

            it 'should call the done with false', (done) ->
                sinon.assert.calledWith(express_done, null, false)
                done()

    describe 'authenticate', ->

        req = res = next = null

        beforeEach (done) ->
            req =
                path: 'blah path'
            next = sinon.spy()
            res = 'blah response'
            done()

        describe 'and the path is not on the whitelist', ->

            authentication_func = null

            beforeEach (done) ->
                # Must reset because the mock is global
                passport_mock.authenticate.reset()
                sinon.stub(sut, 'is_whitelisted').withArgs(req.path).returns(false)
                authentication_func = sinon.spy()
                passport_mock.authenticate.withArgs('basic', session: false).returns(authentication_func)
                sut.authenticate(req, res, next)
                done()

            it 'should authenticate with basic', (done) ->
                sinon.assert.calledWith(passport_mock.authenticate, 'basic', session: false)
                done()

            it 'should call the authentication function with the request, response and next', (done) ->
                sinon.assert.calledWith(authentication_func, req, res, next)
                done()

        describe 'and the path is on the whitelist', ->

            beforeEach (done) ->
                # Must reset because the mock is global
                passport_mock.authenticate.reset()
                sinon.stub(sut, 'is_whitelisted').withArgs(req.path).returns(true)
                sut.authenticate(req, res, next)
                done()

            it 'should call next', (done) ->
                sinon.assert.called(next)
                done()

            it 'should not authenticate with basic', (done) ->
                sinon.assert.notCalled(passport_mock.authenticate)
                done()

    describe 'is_whitelisted', ->

        result = path = null

        describe 'and the path is on the whitelist', ->

            beforeEach (done) ->
                path = 'blah path'
                result = sut.is_whitelisted(path)
                done()

            it 'should return true', (done) ->
                expect(result).to.equal true
                done()

        describe 'and the path is not on the whitelist', ->

            beforeEach (done) ->
                path = 'foo path'
                result = sut.is_whitelisted(path)
                done()

            it 'should return false', (done) ->
                expect(result).to.equal false
                done()

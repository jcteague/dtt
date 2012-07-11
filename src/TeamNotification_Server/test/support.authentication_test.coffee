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

Authentication = module_loader.require('../support/authentication', {
    requires:
        'passport': passport_mock
        'underscore': underscore_mock
        'node_hash': hash_mock
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

        username = password = encrypted = user = name = null

        beforeEach (done) ->
            username = 'jhon@aol.com'
            password = '123456789'
            encrypted = '15e2b0d3c33891ebb0f1ef609ec419420c20e320ce94c65fbc8c3312448eb225'
            name = 'jhonny'
            done()

        describe 'and the user exists on the database', ->

            express_done = null

            beforeEach (done) ->
                hash_mock.sha256.withArgs(password).returns(encrypted)
                user = {id: 1, email: username, password: encrypted, name:name}
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
                    sinon.assert.calledWith(express_done, null, {id: user.id, email: user.email, name:user.name})
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

        beforeEach (done) ->
            sut.authenticate(null, null)
            done()

        it 'should authenticate with basic', (done) ->
            sinon.assert.calledWith(passport_mock.authenticate, 'basic', session: false)
            done()



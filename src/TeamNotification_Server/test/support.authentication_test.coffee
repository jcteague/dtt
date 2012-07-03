expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

passport_mock = 
    use: sinon.stub()
    initialize: sinon.stub()
    authenticate: sinon.stub()

Authentication = module_loader.require('../support/authentication', {
    requires:
        'passport': passport_mock
})

describe 'Authentication', ->

    sut = basic_strategy = repository = null

    beforeEach (done) ->
        sut = new Authentication
        done()

    it 'should tell passport to use the basic strategy', ->
        sinon.assert.calledWith(passport_mock.use, sut.basic_strategy)

    it 'should have instantiated the BasicStrategy', (done) ->
        expect(sut.basic_strategy.constructor.name).to.equal 'BasicStrategy'
        done()

    it 'should have instantiated the repository', (done) ->
        expect(sut.repository.constructor.name).to.equal 'Repository'
        done()

    describe 'initializeAuth', ->

        beforeEach (done) ->
            sut.initializeAuth()
            done()

        it 'should inialize passport', (done) ->
            sinon.assert.called(passport_mock.initialize)
            done()

    describe 'findByUserName', ->

        username = password = express_done = user = null

        beforeEach (done) ->
            username = 'blah user'
            password = 'foo password'
            express_done = sinon.spy()
            done()

        describe 'and the user exists on the database', ->

            beforeEach (done) ->
                user = {id: 2, username: 'blah', password: 1234}
                promise = 
                    then: (callback) ->
                        callback(user)
                sinon.stub(sut.repository, 'find').withArgs(email: username).returns(promise)
                done()

            describe 'and the password matches the username', ->

                beforeEach (done) ->
                    sut.findByUserName(username, password, express_done)
                    done()

                it 'should call the done with the user data', (done) ->
                    sinon.assert.called(express_done, null, user)
                    done()

            describe 'and the password does not match the username', ->

                it 'should call the done with false', (done) ->
                    sinon.assert.calledWith(express_done, null, false)
                    done()

        describe 'and the user does not exist in the database', ->

            beforeEach (done) ->
                promise = 
                    then: (callback) ->
                        callback(null)
                sinon.stub(sut.repository, 'find').withArgs(email: username).returns(promise)

                sut.findByUserName(username, password, express_done)
                done()

            it 'should call the done with false', (done) ->
                sinon.assert.calledWith(express_done, null, false)
                done()

expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

repository_class_mock = sinon.stub()
sut = module_loader.require('../support/strategies/user_by_username_strategy', {
    requires:
        '../repository': repository_class_mock
})

describe 'User By Username Strategy', ->

    describe 'strategy', ->

        users = null
        expected_result = null
        result = null

        beforeEach (done) ->
            repository =
                find: sinon.stub()
            repository_class_mock.withArgs('User').returns(repository)
            users = [{first_name: 'foo'}, {first_name: 'blah'}, {first_name: 'Blah'}, {first_name: 'bL'}, {first_name: 'bar'}]
            promise =
                then: (callback) ->
                    callback(users)
            repository.find.returns(promise)
            done()

        describe 'and the argument is defined', ->

            beforeEach (done) ->
                expected_result = [users[1], users[2], users[3]]
                result = sut('bl')
                done()

            it 'should return the users that start with that username', (done) ->
                expect(result).to.eql expected_result
                done()

        describe 'and the argument is not defined', ->

            beforeEach (done) ->
                expected_result = [users[1], users[2], users[3]]
                result = sut()
                done()

            it 'should return the users that start with that username', (done) ->
                expect(result).to.eql users
                done()

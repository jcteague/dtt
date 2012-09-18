expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

repository_class_mock = sinon.stub()
sut = module_loader.require('../support/strategies/user_by_email_strategy', {
    requires:
        '../repository': repository_class_mock
})

describe 'User By Email Strategy', ->

    describe 'strategy', ->

        users = null
        expected_result = null
        result = null

        beforeEach (done) ->
            repository =
                find: sinon.stub()
            repository_class_mock.withArgs('User').returns(repository)
            users = [{email: 'foo@hello.com'}, {email: 'blah@hello.com'}, {email: 'Blah@hello.com'}, {email: 'bL@hello.com'}, {email: 'bar@hello.com'}]
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
                result = sut()
                done()

            it 'should return all the users', (done) ->
                expect(result).to.eql users
                done()

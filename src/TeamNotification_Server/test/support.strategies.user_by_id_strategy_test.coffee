expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

q_mock =
    when: sinon.spy()

repository_class_mock = sinon.stub()
sut = module_loader.require('../support/strategies/user_by_id_strategy', {
    requires:
        'q': q_mock
        '../repository': repository_class_mock
})

describe 'User By Id Strategy', ->

    describe 'strategy', ->

        expected_result = null
        result = null

        beforeEach (done) ->
            user_id = 100
            repository =
                get_by_id: sinon.stub()
            repository_class_mock.withArgs('User').returns(repository)

            user = 'blah user'
            repository.get_by_id.withArgs(user_id).returns(user)

            expected_result = user
            result = sut(user_id)
            done()

        it 'should return the user repository result for the user id', (done) ->
            expect(result).to.equal expected_result
            done()

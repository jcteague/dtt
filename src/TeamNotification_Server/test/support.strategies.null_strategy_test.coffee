expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

q_mock = 
    fcall: sinon.stub()

sut = module_loader.require('../support/strategies/null_strategy', {
    requires:
        'q': q_mock
})

describe 'Null Strategy', ->

    describe 'strategy', ->

        expected_result = null
        result = null

        beforeEach (done) ->
            q_mock['fcall'] = (callback) ->
                null

            promise = 'blah promise'
            expected_result = null
            result = sut()
            done()

        it 'should return the promise of the value passed', (done) ->
            expect(result).to.equal expected_result
            done()


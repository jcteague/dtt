expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

q_mock = 
    fcall: sinon.spy()

sut = module_loader.require('../support/strategies/pass_through_strategy', {
    requires:
        'q': q_mock
})

describe 'Pass Through Strategy', ->

    describe 'strategy', ->

        expected_result = null
        result = null

        beforeEach (done) ->
            value = 10
            q_mock['fcall'] = (callback) ->
                value

            promise = 'blah promise'
            expected_result = value
            result = sut(value)
            done()

        it 'should return the promise of the value passed', (done) ->
            expect(result).to.equal expected_result
            done()

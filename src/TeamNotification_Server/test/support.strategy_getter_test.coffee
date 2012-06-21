expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

strategies_map_mock = 
    'blah_collection': 'blah strategy path'

StrategyGetter = module_loader.require('../support/strategy_getter', {
    requires:
        './strategies/strategies_map': strategies_map_mock
})

describe 'Strategy Getter', ->

    sut = null

    beforeEach (done) ->
        sut = new StrategyGetter()
        done()

    describe 'get_for', ->

        expected_result = null
        result = null

        beforeEach (done) ->
            type = 'blah_collection'
            strategy = 'blah strategy'
            sinon.stub(sut, 'require_strategy').withArgs(strategies_map_mock[type]).returns(strategy)
            expected_result = strategy
            result = sut.get_for type
            done()

        it 'should return the strategy for that type', (done) ->
            expect(result).to.equal expected_result
            done()


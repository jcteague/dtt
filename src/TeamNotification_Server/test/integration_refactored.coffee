expect = require('expect.js')
sinon = require('sinon')
context = require('./helpers/context')

context.for.integration_test() ->

    describe 'Inner Spec', ->

        result = null

        beforeEach (done) ->
            result = 2 + 2
            done()

        it 'should pass', (done) ->
            expect(result).to.equal 4
            done()


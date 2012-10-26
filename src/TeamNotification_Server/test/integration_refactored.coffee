expect = require('expect.js')
sinon = require('sinon')
#context = require('./helpers/context')

truncate_all_tables = ->
    console.log 'TRUNCATING ALL'

integration_test = (block) ->

    beforeEach (done) ->
        truncate_all_tables()
        done()

    describe 'This is an integration test context', ->
        block()

context =
    for:
        integration_test: integration_test


context.for.integration_test ->

    describe 'Inner Spec', ->

        result = null

        beforeEach (done) ->
            result = 2 + 2
            done()

        it 'should pass', (done) ->
            expect(result).to.equal 4
            done()


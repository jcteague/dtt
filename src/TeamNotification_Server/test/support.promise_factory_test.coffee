expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

promise_class_mock = (class_constructor, promise) ->
    this.promise = promise

PromiseFactory = module_loader.require('../support/promise_factory', {
    requires:
        './promise': promise_class_mock
})

describe 'Promise Factory', ->

    sut = null

    beforeEach (done) ->
        sut = new PromiseFactory()
        done()

    describe 'get_for', ->

        promise = null
        result = null

        beforeEach (done) ->
            promise = 'blah promise'
            class_constructor = 'blah class'
            result = sut.get_for class_constructor, promise
            done()

        it 'should return a promise for the class passed', (done) ->
            expect(result.promise).to.equal promise
            done()

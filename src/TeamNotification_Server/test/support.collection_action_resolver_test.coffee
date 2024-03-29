expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

strategy_getter_class_mock = sinon.stub()
collection_factory_class_mock = sinon.stub()
promise_factory_class_mock = sinon.stub()

CollectionActionResolver = module_loader.require('../support/collection_action_resolver', {
    requires:
        './strategy_getter': strategy_getter_class_mock
        './collection_factory': collection_factory_class_mock
        './promise_factory': promise_factory_class_mock
})

describe 'Collection Action Resolver', ->

    type = null
    strategy = null
    collection_class = null
    promise_factory = null
    sut = null

    beforeEach (done) ->
        type = 'user_collection'

        strategy_getter = 
            get_for: sinon.stub()
        strategy_getter_class_mock.returns(strategy_getter)

        strategy = sinon.stub()
        strategy_getter.get_for.withArgs(type).returns(strategy)

        collection_factory =
            get_for: sinon.stub()
        collection_factory_class_mock.returns(collection_factory)

        promise_factory =
            get_for: sinon.stub()
        promise_factory_class_mock.returns(promise_factory)

        collection_class = 'blah collection class'
        collection_factory.get_for.withArgs(type).returns(collection_class)
        sut = new CollectionActionResolver type
        done()

    describe 'constructor', ->

        it 'should contain a property for the type passed', (done) ->
            expect(sut.type).to.equal type
            done()

        it 'should get the strategy needed for that collection', (done) ->
            expect(sut.strategy).to.equal strategy
            done()

        it 'should get the collection class needed for that collection type', (done) ->
            expect(sut.collection_class).to.equal collection_class
            done()

        it 'should instantiate a promise factory', (done) ->
            expect(promise_factory_class_mock.calledWithNew()).to.be true
            expect(sut.promise_factory).to.equal promise_factory
            done()

    describe 'for', ->

        strategy_promise = null
        expected_result = null
        result = null
        parameters = null

        beforeEach (done) ->
            parameters = 'foo bar'
            strategy_promise = 'blah strategy result'

            sut.collection_class = sinon.spy()
            strategy.withArgs(parameters).returns(strategy_promise)

            collection_promise = 
                then: (callback) ->
                    callback(strategy_promise)

            sut.promise_factory.get_for.withArgs(sut.collection_class, strategy_promise).returns(collection_promise)

            expected_result = collection_promise
            result = sut.for parameters
            done()

        it 'should return a collection promise', (done) ->
            expect(result).to.equal expected_result
            done()

    describe 'fetch_to', ->

        callback = null
        collection_promise = null

        beforeEach (done) ->
            sut.collection_class = sinon.spy()
            sut.promise_factory = 
                get_for: sinon.stub()

            strategy_promise = 'blah strategy result'
            strategy.returns(strategy_promise)

            collection_promise = 
                fetch_to: sinon.spy()

            sut.promise_factory.get_for.withArgs(sut.collection_class, strategy_promise).returns(collection_promise)

            sut.fetch_to callback
            done()

        it 'should call the fetch_to method of the promise created by the promise factory', (done) ->
            sinon.assert.calledWith(collection_promise.fetch_to, callback)
            done()

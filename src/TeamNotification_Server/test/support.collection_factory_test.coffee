expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

collection_mock = sinon.spy()
CollectionFactory = module_loader.require('../support/collection_factory', {
    requires:
        './collections/user_collection': collection_mock
})

describe 'Collection Factory', ->

    type = null
    sut = null

    beforeEach (done) ->
        type = 'user_collection'
        sut = new CollectionFactory type
        done()

    describe 'constructor', ->

        it 'should contain a property for the type passed', (done) ->
            expect(sut.type).to.equal type
            done()

    describe 'for', ->

        options = null

        beforeEach (done) ->
            result = sut.for options
            done()

        it 'should return an instance of the collection type of the factory with the arguments passed', (done) ->
            expect(collection_mock.calledWithNew()).to.be true
            sinon.assert.calledWith(collection_mock, options)
            done()

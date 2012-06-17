expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

collection_factory_mock = sinon.spy()

sut = module_loader.require('../support/routes_service', {
    requires:
        './collection_factory': collection_factory_mock
})

describe 'Routes Service', ->

    describe 'build', ->

        collection_type = null
        result = null

        beforeEach (done) ->
            result = sut.build collection_type
            done()

        it 'should return a collection factory using the collection type passed', (done) ->
            expect(collection_factory_mock.calledWithNew()).to.be true
            sinon.assert.calledWith(collection_factory_mock, collection_type)
            done()


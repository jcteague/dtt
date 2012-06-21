expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

collections_map_mock = 
    'blah_collection': 'blah collection class path'

CollectionFactory = module_loader.require('../support/collection_factory', {
    requires:
        './collections/collections_map': collections_map_mock
        'path': {}
})

describe 'Collection Factory', ->

    sut = null

    beforeEach (done) ->
        sut = new CollectionFactory()
        done()

    describe 'get_for', ->

        expected_result = null
        result = null

        beforeEach (done) ->
            type = 'blah_collection'
            collection_class = 'blah collection'
            sinon.stub(sut, 'require_collection').withArgs(collections_map_mock[type]).returns(collection_class)
            expected_result = collection_class
            result = sut.get_for type
            done()

        it 'should return the collection for that type', (done) ->
            expect(result).to.equal expected_result
            done()

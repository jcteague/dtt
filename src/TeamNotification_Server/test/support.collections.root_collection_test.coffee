expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

RootCollection = module_loader.require('../support/collections/root_collection', {})

describe 'Root Collection', ->

    sut = null

    beforeEach (done) ->
        sut = new RootCollection()
        done()

    describe 'to_json', ->

        result = null

        beforeEach (done) ->
            result = sut.to_json()
            done()

        it 'should return as a json all the links for the root path', (done) ->
            links = result['links'] 
            expect(links[0]).to.eql {"name": "self", "rel": "self", "href": "/"}
            expect(links[1]).to.eql {"name": "user", "rel": "User", "href": "/user"}
            done() 

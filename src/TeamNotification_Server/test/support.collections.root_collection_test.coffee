expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

RootCollection = module_loader.require('../support/collections/root_collection', {})

describe 'Root Collection', ->

    sut = user_id = null

    beforeEach (done) ->
   #     user_id = 3
        sut = new RootCollection()
        done()

#    describe 'constructor', ->

 #       it 'should set the collection with the constructor values', (done) ->
 #           expect(sut.user_id).to.equal user_id
  #          done()

    describe 'to_json', ->

        result = null

        beforeEach (done) ->
            result = sut.to_json()
            done()

        it 'should return as a json all the links for the root path', (done) ->
            links = result['links'] 
            expect(links[0]).to.eql {"name": "self", "rel": "self", "href": "/"}
            expect(links[1]).to.eql {"name": "Login", "rel": "login", "href": "/user/login"}
            expect(links[2]).to.eql {"name": "Registration", "rel": "registration", "href": "/registration"}
            done() 

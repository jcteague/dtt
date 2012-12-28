expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

json_mock = sinon.stub()
registration_collection_mock = ()->
    to_json: json_mock
login_form_collection_mock = ()->
    to_json: json_mock

RootCollection = module_loader.require('../support/collections/root_collection', {
    requires:
        './registration_collection': registration_collection_mock
        './login_form_collection': login_form_collection_mock
})

describe 'Root Collection', ->

    sut = user_id = null

    beforeEach (done) ->
        sut = new RootCollection()
        done()

    describe 'to_json', ->

        result = expected_json = null

        beforeEach (done) ->
            expected_json = "Expected"
            json_mock.returns(expected_json)
            result = sut.to_json()
            done()

        it 'should return as a json with all the links for the root path', (done) ->
            links = result.root['links'] 
            expect(links[0]).to.eql {"name": "self", "rel": "self", "href": "/"}
            expect(links[1]).to.eql {"name": "Login", "rel": "login", "href": "/user/login"}
            expect(links[2]).to.eql {"name": "Registration", "rel": "registration", "href": "/registration"}
            done()
            
        it 'should return a json with the registration collection json', (done) ->
            expect(result.registration).to.eql expected_json 
            done()
        it 'should return a json with the login collection json', (done) ->
            expect(result.login).to.eql expected_json 
            done()

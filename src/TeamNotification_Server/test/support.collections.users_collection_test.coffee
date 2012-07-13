expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

UsersCollection = module_loader.require('../support/collections/users_collection', {})

describe 'Users Collection', ->

    sut = null
    users = null

    beforeEach (done) ->
        users = [ { id:1, first_name: 'foo'},{id:2, first_name:'bar'}]
        sut = new UsersCollection users
        done()

    describe 'constructor', ->

        it 'should have the users value set inside the object', (done) ->
            expect(sut.users).to.equal(users)
            done()

    describe 'to_json', ->

        result  = null

        beforeEach (done) ->
            sut.users = users
            result = sut.to_json()
            done()
        
        it 'should return the users collection links as json', (done) ->
            links = result['links']
            expect(links[0]).to.eql {"name":"self", "rel": "Users", "href": "/users/query"}
            expect(links[1]).to.eql {"name":"#{users[0].first_name}",  "rel": "User", "href": "/user/#{users[0].id}"}
            expect(links[2]).to.eql {"name":"#{users[1].first_name}",  "rel": "User", "href": "/user/#{users[1].id}"}
            done()

        it 'should return the users collection users in the resulting json', (done) ->
            data = result['users']
            expect(data[0]).to.eql {"href": "/user/#{users[0].id}", "data": [{"name": "id", "value": users[0].id}, {"name": "name", "value": users[0].first_name}]}
            expect(data[1]).to.eql {"href": "/user/#{users[1].id}", "data": [{"name": "id", "value": users[1].id}, {"name": "name", "value": users[1].first_name}]}
            done()


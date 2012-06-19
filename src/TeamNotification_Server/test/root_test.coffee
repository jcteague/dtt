should = require('should')
sinon = require('sinon')

root = require('../routes/root.js')

describe 'Root', ->

    describe 'build_routes', ->
        app  = null

        beforeEach (done) ->
            app = { get:sinon.spy() } 

            root.build_routes(app)
            done() 

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get,'/',root.methods.get_root) 
            done() 

    describe 'methods', ->

        describe 'get_root', ->
            app = null
            json_data = null

            beforeEach (done) ->
                res = 
                    json: (json) -> json_data = json
                app = get:sinon.spy()

                root.methods.get_root({},res)
                done()

            it 'should return as a json all the links for the root path', (done) ->
                links = json_data['links'] 
                links[0].should.eql {"name": "self", "rel": "self", "href": "/"}
                links[1].should.eql {"name": "user", "rel": "User", "href": "/user"}
                done() 

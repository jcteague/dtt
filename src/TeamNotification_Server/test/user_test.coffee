should = require('should')
sinon = require('sinon')
user = require('../routes/user.js')

describe 'User', ->

    describe 'build_routes', ->
        app = null

        beforeEach (done) ->
            app = { get:sinon.spy() }
            user.build_routes(app)
            done()

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get,'/user',user.methods.get_user)
            sinon.assert.calledWith(app.get,'/user/rooms',user.methods.get_user_rooms)
            done()

    describe 'methods', ->
        
        describe 'get_user', ->
            app = null
            json_data = null

            beforeEach (done) ->
                res = 
                    json: (json) -> json_data = json
                app = { get:sinon.spy() }

                user.methods.get_user({},res)
                done()

            it 'should return the correct links for the user model', (done) ->
                links = json_data['links']
                links[0].should.eql {"rel": "self", "href": "/user"}
                links[1].should.eql {"rel": "rooms", "href": "/user/rooms"}
                done()

        describe 'get_user_rooms', ->

            app = null
            json_data = null

            beforeEach (done) ->
                res = 
                    json: (json) -> json_data = json
                app = { get:sinon.spy() }

                user.methods.get_user_rooms({},res)
                done()

            it 'should return the correct links for the user rooms model', (done) ->
                links = json_data['links']
                links[0].should.eql {"rel": "self", "href": "/user/rooms"}
                links[1].should.eql {"rel": "OpenRoom1", "href": "/rooms/1"}
                done()

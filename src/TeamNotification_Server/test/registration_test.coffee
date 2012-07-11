expect = require('should')
sinon = require('sinon')
module_loader = require('sandboxed-module')

routes_service_mock =
    build: sinon.stub()

repository_class_mock = sinon.stub()
node_hash_mock =
    sha256: sinon.stub()

sut = module_loader.require('../routes/registration', {
    requires:
        '../support/routes_service': routes_service_mock
        '../support/repository': repository_class_mock
        'node_hash': node_hash_mock
})

describe 'Registration', ->

    describe 'build_routes', ->
        app  = null

        beforeEach (done) ->
            app = { get:sinon.spy(), post: sinon.spy()} 

            sut.build_routes(app)
            done() 

        it 'should configure the routes with its corresponding callback', (done) ->
            sinon.assert.calledWith(app.get, '/registration', sut.methods.get_registration) 
            sinon.assert.calledWith(app.post, '/registration', sut.methods.post_registration) 
            done() 

    describe 'methods', ->

        res = req = null

        describe 'get_registration', ->

            collection_value = null

            beforeEach (done) ->
                collection_value = 'blah collection'
                collection =
                    to_json: ->
                        collection_value

                collection_action =
                    fetch_to: (callback) ->
                        callback(collection)

                routes_service_mock.build.withArgs('registration_collection').returns(collection_action)
                res = 
                    json: sinon.spy()
                req = {}
                sut.methods.get_registration(req, res)
                done()

            it 'should return the built collection for the user model', (done) ->
                sinon.assert.calledWith(res.json, collection_value)
                done()

        describe 'post_registration', ->

            repository = is_valid_user_stub = null

            beforeEach (done) ->
                is_valid_user_stub = sinon.stub(sut.methods, 'is_valid_user')
                repository =
                    save: sinon.stub()
                repository_class_mock.withArgs('User').returns(repository)

                req.body = 
                    first_name: 'foo'
                    last_name: 'bar'
                    email: 'foo@bar.com'
                    password: '1234'

                res =
                    json: sinon.spy()

                done()

            afterEach (done) ->
                sut.methods.is_valid_user.restore()
                done()

            describe 'and the values sent are valid', ->

                repository = user_data = save_promise = saved_user = null

                beforeEach (done) ->
                    is_valid_user_stub.withArgs(req.body).returns(true)
                    repository =
                        save: sinon.stub()
                    repository_class_mock.withArgs('User').returns(repository)

                    hashed_password = 'abcd'
                    node_hash_mock.sha256.withArgs(req.body.password).returns(hashed_password)
                    user_data =
                        first_name: req.body.first_name
                        last_name: req.body.last_name
                        email: req.body.email
                        password: hashed_password

                    saved_user =
                        id: 10
                    save_promise =
                        then: (callback) ->
                            callback(saved_user)
                    repository.save.withArgs(user_data).returns(save_promise)
                    sut.methods.post_registration(req, res)
                    done()
                
                it 'should create a record for the user passed', (done) ->
                    sinon.assert.calledWith(repository.save, user_data)
                    done()

                it 'should notify the user that the user was created', (done) ->
                    sinon.assert.calledWith(res.json, {success: true, data: saved_user})
                    done()

            describe 'and the values sent are not valid', ->

                user_data = save_promise = null

                beforeEach (done) ->
                    is_valid_user_stub.withArgs(req.body).returns(false)
                    hashed_password = 'abcd'
                    node_hash_mock.sha256.withArgs(req.body).returns(hashed_password)
                    user_data =
                        first_name: req.body.first_name
                        last_name: req.body.last_name
                        email: req.body.email
                        password: hashed_password

                    saved_user =
                        id: 10
                    save_promise =
                        then: (callback) ->
                            callback(saved_user)
                    repository.save.withArgs(user_data).returns(save_promise)
                    sut.methods.post_registration(req, res)
                    done()
                
                it 'should not create a record for the user passed', (done) ->
                    sinon.assert.notCalled(repository.save)
                    done()

                it 'should notify the user that the user data was invalid', (done) ->
                    sinon.assert.calledWith(res.json, {success: false, message: 'user data is invalid'})
                    done()

        describe 'is_valid_user', ->

            result = req = user_data = errors = null

            beforeEach (done) ->
                req =
                    assert: sinon.spy()
                done()

            describe 'and there are validation errors', ->

                beforeEach (done) ->
                    user_data =
                        first_name: 'foo'
                        last_name: 'bar'
                        email: 'foo@bar.com'
                        password: '1234'

                    result = sut.methods.is_valid_user(req, user_data)
                    done()

                it 'should return false', (done) ->
                    expect(result.valid).to.equal false
                    done()

            describe 'and there are no validation errors', ->

                beforeEach (done) ->
                    user_data =
                        first_name: 'foo'
                        last_name: 'bar'
                        email: 'foocom'
                        password: '1234'
                    result = sut.methods.is_valid_user(req, user_data)
                    done()

                it 'should return true', (done) ->
                    expect(result.valid).to.equal true
                    done()

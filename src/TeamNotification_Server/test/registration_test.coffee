expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

routes_service_mock =
    build: sinon.stub()

repository_class_mock = sinon.stub()
user_repository =
    save: sinon.stub()
repository_class_mock.withArgs('User').returns(user_repository)

node_hash_mock =
    sha256: sinon.stub()

q_mock =
    when: sinon.stub()

sut = module_loader.require('../routes/registration', {
    requires:
        '../support/routes_service': routes_service_mock
        '../support/repository': repository_class_mock
        'node_hash': node_hash_mock
        'q': q_mock
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

            res = is_valid_user_stub = null

            beforeEach (done) ->
                is_valid_user_stub = sinon.stub(sut.methods, 'is_valid_user')

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

                user_data = save_promise = saved_user = is_email_already_registered_stub = is_registered = email_registered_handler = null

                beforeEach (done) ->
                    is_registered = 'blah registered'
                    sinon.stub(sut.methods, 'is_email_already_registered').withArgs(req.body.email).returns(is_registered)
                    
                    email_registered_handler = 'blah handler'
                    sinon.stub(sut.methods, 'get_email_registered_handler').withArgs(req, res).returns(email_registered_handler)
                    is_valid_user_stub.withArgs(req.body).returns(valid: true, errors: [])
                    sut.methods.post_registration(req, res)
                    done()

                afterEach (done) ->
                    sut.methods.is_email_already_registered.restore()
                    sut.methods.get_email_registered_handler.restore()
                    done()

                it 'should call the Q when with the is email registered result and its handler', (done) ->
                    sinon.assert.calledWith(q_mock.when, is_registered, email_registered_handler)
                    done()

            describe 'and the values sent are not valid', ->

                user_data = save_promise = errors = null

                beforeEach (done) ->
                    errors = ['blah error message']
                    is_valid_user_stub.withArgs(req.body).returns(valid: false, errors: errors)
                    sut.methods.post_registration(req, res)
                    done()
                
                it 'should notify the user that the user data was invalid', (done) ->
                    sinon.assert.calledWith(res.json, {success: false, messages: errors})
                    done()

        describe 'get_email_registered_handler', ->

            req = res = null

            beforeEach (done) ->
                res =
                    json: sinon.stub()

                req.body = 
                    first_name: 'foo'
                    last_name: 'bar'
                    email: 'foo@bar.com'
                    password: '1234'

                done()

            describe 'and the email is registered', ->

                beforeEach (done) ->
                    callback = sut.methods.get_email_registered_handler(req, res)
                    callback(true)
                    done()

                it 'should not create a record for the user passed', (done) ->
                    sinon.assert.notCalled(user_repository.save)
                    done()

                it 'should notify the user that the email was already registered', (done) ->
                    sinon.assert.calledWith(res.json, {success: false, messages: ['email is already registered']})
                    done()

            describe 'and the email is not registered', ->

                save_promise = saved_user = user_data = null

                beforeEach (done) ->
                    user_repository.save.reset()

                    hashed_password = 'abcdefgh'
                    node_hash_mock.sha256.withArgs(req.body.password).returns(hashed_password)
                    user_data =
                        first_name: req.body.first_name
                        last_name: req.body.last_name
                        email: req.body.email
                        password: hashed_password

                    saved_user =
                        id: 99
                        email: 'foo@bar.com'
                    save_promise =
                        then: (callback) ->
                            callback(saved_user)
                    user_repository.save.withArgs(user_data).returns(save_promise)
                    callback = sut.methods.get_email_registered_handler(req, res)
                    callback(false)
                    done()

                it 'should save the user in the repository', (done) ->
                    sinon.assert.calledWith(user_repository.save, user_data)
                    done()

                it 'should notify the user that the user was created', (done) ->
                    sinon.assert.calledWith(res.json, {success: true, data: {id: saved_user.id, email: saved_user.email}})
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
                        email: 'foobar.com'
                        password: '123456'

                    result = sut.methods.is_valid_user(user_data)
                    done()

                it 'should return false', (done) ->
                    expect(result.valid).to.equal false
                    done()

            describe 'and there are no validation errors', ->

                beforeEach (done) ->
                    user_data =
                        first_name: 'foo'
                        last_name: 'bar'
                        email: 'foo@bar.com'
                        password: '123456'
                    result = sut.methods.is_valid_user(user_data)
                    done()

                it 'should return true', (done) ->
                    expect(result.valid).to.equal true
                    done()

        ###
        describe 'is_email_already_registered', ->

            result = email = null

            beforeEach (done) ->
                email = 'foo@bar.com'
                done()

            describe 'and the email is already registered', ->

                beforeEach (done)
                    result = sut.methods.is_email_already_registered(email)
                    done()

                it 'should return true', (done) ->
                    done()
        ###

expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

routes_service_mock =
    build: sinon.stub()

registration_validator_mock =
    validate: sinon.stub()

registration_callback_factory_mock =
    get_for_success: sinon.stub()
    get_for_failure: sinon.stub()

sut = module_loader.require('../subdomains/api/routes/registration', {
    requires:
        '../../../support/routes_service': routes_service_mock
        '../../../support/validation/registration_validator': registration_validator_mock
        '../../../support/factories/registration_callback_factory': registration_callback_factory_mock
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

            success_callback = failure_callback = validation_result = null

            beforeEach (done) ->
                req = body: 'blah req body'
                res = 'blah res'

                success_callback = 'success callback'
                registration_callback_factory_mock.get_for_success.withArgs(req, res).returns(success_callback)
                failure_callback = 'failure callback'
                registration_callback_factory_mock.get_for_failure.withArgs(req, res).returns(failure_callback)

                validation_result =
                    handle_with: sinon.spy()
                registration_validator_mock.validate.withArgs(req.body).returns(validation_result)
                    
                sut.methods.post_registration(req, res)
                done()

            it 'should validate the registration and handle it with the success and failure callbacks', (done) ->
                sinon.assert.calledWith(validation_result.handle_with, success_callback, failure_callback)
                done()

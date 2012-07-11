RegistrationCollection = module_loader.require('../support/collections/registration_collection', {})

describe 'Registration Collection', ->

    sut = null

    beforeEach (done) ->
        sut = new RegistrationCollection()
        done()

    describe 'to_json', ->

        result = null

        beforeEach (done) ->
            result = sut.to_json()
            done()

        it 'should return a template with the name, email, password and confirm password fields', (done) ->
            done()

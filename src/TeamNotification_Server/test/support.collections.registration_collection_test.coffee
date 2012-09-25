expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

RegistrationCollection = module_loader.require('../support/collections/registration_collection', {})

describe 'Registration Collection', ->

    sut = data = null

    beforeEach (done) ->
        data =
           first_name: 'foo'
           last_name: 'bar'
           email: 'foo@bar.com'

        sut = new RegistrationCollection(data)
        done()

    describe 'constructor', ->

        it 'should set the collection with the constructor values', (done) ->
            expect(sut.data).to.equal data
            done()

    describe 'to_json', ->
        
        result = null

        describe 'and there is no data stored', ->

            beforeEach (done) ->
                sut.data = null
                result = sut.to_json()
                done()

            it 'should return the href property pointing to the current url', (done) ->
                expect(result['href']).to.eql "/registration"
                done()

            it 'should return the list of links', (done) ->
                links = result['links']
                expect(links[0]).to.eql {"name": "self", "rel": "Registration", "href": "/registration"}
                done()

            it 'should return a template type of registration', (done) ->
                expect(result['template']['type']).to.equal 'registration'
                done()

            it 'should return a template with the first name, last name, email, password and confirm password fields', (done) ->
                template_data = result['template']['data']
                expect(template_data[0]).to.eql {'name': 'first_name', 'label': 'First Name', 'type': 'string', 'value': '', 'rules': {'required': true}}
                expect(template_data[1]).to.eql {'name': 'last_name', 'label': 'Last Name', 'type': 'string', 'value': '', 'rules': {'required': true}}
                expect(template_data[2]).to.eql {'name': 'email', 'label': 'Email', 'type': 'string', 'value': '', 'rules': {'required': true, 'email': true}}
                expect(template_data[3]).to.eql {'name': 'password', 'label': 'Password', 'type': 'password', 'value': '', 'rules': {'required': true, 'minlength': 6}}
                expect(template_data[4]).to.eql {'name': 'confirm_password', 'label': 'Confirm Password', 'type': 'password', 'value': '', 'rules': {'required': true, 'minlength': 6, 'equalTo': 'password'}}
                done()


        describe 'and there is data stored', ->

            beforeEach (done) ->
               sut.data = data
               result = sut.to_json()
               done()

            it 'should return the href property pointing to the current url', (done) ->
                expect(result['href']).to.eql "/registration"
                done()

            it 'should return the list of links', (done) ->
                links = result['links']
                expect(links[0]).to.eql {"name": "self", "rel": "Registration", "href": "/registration"}
                done()

            it 'should return a template type of registration', (done) ->
                expect(result['template']['type']).to.equal 'registration'
                done()

            it 'should return a template filled with the values filled from the data', (done) ->
                template_data = result['template']['data']
                expect(template_data[0]).to.eql {'name': 'first_name', 'label': 'First Name', 'type': 'string', 'value': data.first_name, 'rules': {'required': true}}
                expect(template_data[1]).to.eql {'name': 'last_name', 'label': 'Last Name', 'type': 'string', 'value': data.last_name, 'rules': {'required': true}}
                expect(template_data[2]).to.eql {'name': 'email', 'label': 'Email', 'type': 'string', 'value': data.email, 'rules': {'required': true, 'email': true}}
                expect(template_data[3]).to.eql {'name': 'password', 'label': 'Password', 'type': 'password', 'value': '', 'rules': {'required': true, 'minlength': 6}}
                expect(template_data[4]).to.eql {'name': 'confirm_password', 'label': 'Confirm Password', 'type': 'password', 'value': '', 'rules': {'required': true, 'minlength': 6, 'equalTo': 'password'}}
                done()


    describe 'fill', ->

        result = filled_data = null

        beforeEach (done) ->
            filled_data =
                first_name: 'blah'

            result = sut.fill filled_data
            done()

        it 'should return a clone of the collection with the passed properties', (done) ->
            expect(result).not.to.equal sut
            expect(result.data).to.eql {first_name: filled_data.first_name, last_name: data.last_name, email: data.email}
            done()

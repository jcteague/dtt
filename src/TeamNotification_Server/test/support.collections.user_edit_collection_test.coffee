expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

UserEditCollection = module_loader.require('../support/collections/user_edit_collection', {})

describe 'User Edit Collection', ->

    sut = data = user_id = null

    beforeEach (done) ->
        user_id = 8
        data =
            id: user_id
            first_name: 'foo'
            last_name: 'bar'
            email: 'foo@bar.com'
            password: '123456'
        sut = new UserEditCollection(data)
        done()

    describe 'constructor', ->

        it 'should set the collection with the constructor values', (done) ->
            expect(sut.data).to.equal data
            done()

    describe 'to_json', ->

        result = null

        beforeEach (done) ->
            sut.data = data
            result = sut.to_json()
            done()

        it 'should return a href property pointing to the current url', (done) ->
            expect(result['href']).to.equal "/user/#{user_id}/edit"
            done()

        it 'should set the correct links for the user model', (done) ->
            links = result['links']
            expect(links[0]).to.eql {"rel": "UserEdit", "name": "self", "href": "/user/#{user_id}/edit"}
            done()

        it 'should return a template type of user edit', (done) ->
            expect(result['template']['type']).to.equal 'user_edit'
            done()

        it 'should return a template with the first name, last name, email, password and confirm password fields', (done) ->
            template_data = result['template']['data']
            expect(template_data[0]).to.eql {'name': 'id', 'value': data.id, 'type': 'hidden', 'rules': {'required': true}}
            expect(template_data[1]).to.eql {'name': 'first_name', 'label': 'First Name', 'value': data.first_name, 'type': 'string', 'rules': {'required': true}}
            expect(template_data[2]).to.eql {'name': 'last_name', 'label': 'Last Name', 'value': data.last_name, 'type': 'string', 'rules': {'required': true}}
            expect(template_data[3]).to.eql {'name': 'email', 'label': 'Email', 'value': data.email, 'type': 'string', 'rules': {'required': true, 'email': true}}
            expect(template_data[4]).to.eql {'name': 'password', 'label': 'Password', 'type': 'password', 'rules': {'required': false, 'minlength': 6}}
            expect(template_data[5]).to.eql {'name': 'confirm_password', 'label': 'Confirm Password', 'type': 'password', 'rules': {'required': false, 'minlength': 6, 'equalTo': 'password'}}
            done()
